using ArticleProject.Model.Models.CommonModels.OutputModels;
using ArticleProject.Model.Models.UserModels;
using MediatR;

namespace ArticleProject.Api.Application.Commands.AuthenticationCommands
{
    public class RegisterCommand : IRequest<MessageOutputModel>
    {
        public UserRegisterModel Model;

        public RegisterCommand(UserRegisterModel model)
        {
            Model = model;
        }
    }
}