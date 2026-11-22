using ArticleProject.Model.Models.CommonModels.OutputModels;
using ArticleProject.Model.Models.UserModels;
using MediatR;

namespace ArticleProject.Api.Application.Commands.AuthenticationCommands
{
    public class ResetPasswordCommand : IRequest<MessageOutputModel>
    {
        public UserPasswordResetModel Model { get; set; }

        public ResetPasswordCommand(UserPasswordResetModel request)
        {
            Model = request;
        }
    }
}