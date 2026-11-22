using ArticleProject.Model.Models.CommonModels.OutputModels;
using ArticleProject.Model.Models.UserManagementModels;
using MediatR;

namespace ArticleProject.Api.Application.Commands.UserManagementCommands
{
    public class AssignUserResearcherCommand : IRequest<MessageOutputModel>
    {
        public UserResearcherAssignInputModel Model { get; set; }

        public AssignUserResearcherCommand(UserResearcherAssignInputModel request)
        {
            Model = request;
        }
    }
}