using MediatR;

namespace ArticleProject.Api.Application.Commands.ScopusServiceCommands
{
    public class FetchResearchersDataScopusCommand : IRequest<bool>
    { }
}