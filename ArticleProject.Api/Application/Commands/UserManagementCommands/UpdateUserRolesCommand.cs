using ArticleProject.Model.Models.UserManagementModels;
using MediatR;

namespace ArticleProject.Api.Application.Commands.UserManagementCommands
{
    public class UpdateUserRolesCommand : IRequest<UserDataOutputModel>
    {
        public UserRoleUpdateInputModel Model { get; set; }

        public UpdateUserRolesCommand(UserRoleUpdateInputModel request)
        {
            Model = request;
        }
    }
}