using ArticleProject.Domain.Aggregates;
using ArticleProject.Domain.Common.Exceptions;
using ArticleProject.Domain.Common.Interfaces.Repository;
using ArticleProject.Model.Models.CommonModels.OutputModels;
using ArticleProject.Resources;
using MediatR;
using Microsoft.Extensions.Localization;

namespace ArticleProject.Api.Application.Commands.JournalCommands
{
    public class UpdateJournalCommandHandler : IRequestHandler<UpdateJournalCommand, KeyOutputModel>
    {
        private readonly IRepository<Journal> _journalRepository;
        private readonly IRepository<Category> _categoryRepository;
        private readonly IStringLocalizer<JournalRes> _stringLocalizer;

        public UpdateJournalCommandHandler(IRepository<Journal> journalRepository, IRepository<Category> categoryRepository, IStringLocalizer<JournalRes> stringLocalizer)
        {
            _journalRepository = journalRepository;
            _stringLocalizer = stringLocalizer;
            _categoryRepository = categoryRepository;
        }

        public async Task<KeyOutputModel> Handle(UpdateJournalCommand request, CancellationToken cancellationToken)
        {
            var response = new KeyOutputModel();

            // Check uniqueness rule
            var existingEntityCount = _journalRepository.GetCountByFilter(p => p.Id != request.Model.Id && (p.Name == request.Model.Name));

            if (existingEntityCount > 0)
            {
                response.IsSuccessed = false;
                response.Messages.Add(_stringLocalizer["ErrorJournalMustBeUniqueToUpdate"]);
                return response;
            }

            var entity = (await _journalRepository.GetByIdAsync(request.Model.Id)).ThrowIfNull(_stringLocalizer["ErrorNoJournalRecordFound"])!;
            var category = (await _categoryRepository.GetByIdAsync(entity.CategoryId)).ThrowIfNull();
            category.Update(request.Model.CategoryName, request.Model.Quartile, request.Model.Rank, request.Model.TotalJournals);
            entity.Update(request.Model.Name, request.Model.AbbreviationName, request.Model.ISSN, request.Model.EISSN, request.Model.JIF, request.Model.JIF5Years, request.Model.Year, category);

            await _journalRepository.UpdateAndSaveAsync(entity);

            response.Id = entity.Id;
            response.IsSuccessed = true;
            response.Messages.Add(_stringLocalizer["JournalUpdatedSuccessfully"]);
            return response;
        }
    }
}