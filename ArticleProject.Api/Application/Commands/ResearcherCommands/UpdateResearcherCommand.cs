using ArticleProject.Model.Models.ResearcherModels;
using MediatR;

namespace ArticleProject.Api.Application.Commands.ResearcherCommands
{
    public class UpdateResearcherCommand : IRequest<ResearcherDataOutputModel>
    {
        public ResearcherUpdateInputModel Model { get; set; }

        public UpdateResearcherCommand(ResearcherUpdateInputModel request)
        {
            Model = request;
        }
    }
}