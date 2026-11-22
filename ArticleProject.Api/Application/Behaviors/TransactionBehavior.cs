using ArticleProject.Data.ArticleProjectDbContext;
using ArticleProject.Resources;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace ArticleProject.Api.Application.Behaviors
{
    public class TransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly ArticleProjectContext _dbContext;
        private readonly IStringLocalizer<CommonRes> _stringLocalizer;

        public TransactionBehavior(ArticleProjectContext dbContext, IStringLocalizer<CommonRes> stringLocalizer)
        {
            _dbContext = dbContext;
            _stringLocalizer = stringLocalizer;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var strategy = _dbContext.Database.CreateExecutionStrategy();

            return await strategy.ExecuteAsync(async () =>
            {
                await using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
                try
                {
                    var response = await next();

                    await _dbContext.SaveChangesAsync(cancellationToken);
                    await transaction.CommitAsync(cancellationToken);

                    return response;
                }
                catch (NullReferenceException)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    throw;
                }
                catch (ValidationException)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    throw;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    throw new Exception(_stringLocalizer["ErrorDBTransactionFailed"], ex);
                }
            });
        }
    }
}