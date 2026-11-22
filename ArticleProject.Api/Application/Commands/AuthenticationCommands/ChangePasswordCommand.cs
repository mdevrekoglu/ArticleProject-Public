using ArticleProject.Model.Models.CommonModels.OutputModels;
using ArticleProject.Model.Models.UserModels;
using MediatR;

namespace ArticleProject.Api.Application.Commands.AuthenticationCommands
{
    public class ChangePasswordCommand : IRequest<MessageOutputModel>
    {
        public UserChangePasswordModel Model { get; set; }

        public ChangePasswordCommand(UserChangePasswordModel request)
        {
            Model = request;
        }
    }
}