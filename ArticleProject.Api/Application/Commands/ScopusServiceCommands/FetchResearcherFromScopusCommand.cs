using ArticleProject.Model.Models.CommonModels.InputModels;
using ArticleProject.Model.Models.CommonModels.OutputModels;
using MediatR;

namespace ArticleProject.Api.Application.Commands.ScopusServiceCommands
{
    public class FetchResearcherFromScopusCommand : IRequest<MessageOutputModel>
    {
        public KeyInputModel Model { get; set; }

        public FetchResearcherFromScopusCommand(KeyInputModel request)
        {
            Model = request;
        }
    }
}