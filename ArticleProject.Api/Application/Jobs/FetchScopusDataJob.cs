using ArticleProject.Api.Application.Commands.ScopusServiceCommands;
using ArticleProject.Domain.Common.Extensions;
using MediatR;

namespace ArticleProject.Api.Application.Jobs
{
    public class FetchScopusDataJob : CronJobExtensions
    {
        public FetchScopusDataJob(IScheduleConfig<FetchScopusDataJob> config, IServiceProvider serviceProvider)
                                    : base(config.CronExpression, config.TimeZoneInfo, serviceProvider)
        { }

        public override async Task DoWork(IServiceScope scope, CancellationToken cancellationToken)
        {
            using (scope)
            {
                var command = new FetchResearchersDataScopusCommand();
                await scope.ServiceProvider.GetRequiredService<IMediator>().Send(command);
            }
        }
    }
}