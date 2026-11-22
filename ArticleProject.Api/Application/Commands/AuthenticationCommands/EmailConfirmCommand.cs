using ArticleProject.Model.Models.CommonModels.OutputModels;
using ArticleProject.Model.Models.UserModels;
using MediatR;

namespace ArticleProject.Api.Application.Commands.AuthenticationCommands
{
    public class EmailConfirmCommand : IRequest<MessageOutputModel>
    {
        public UserConfirmEmailModel Model { get; set; }

        public EmailConfirmCommand(UserConfirmEmailModel model)
        {
            Model = model;
        }
    }
}