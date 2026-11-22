using ArticleProject.Model.Models.CommonModels.OutputModels;
using ArticleProject.Model.Models.UserModels;
using MediatR;

namespace ArticleProject.Api.Application.Commands.AuthenticationCommands
{
    public class ChangeUserImageCommand : IRequest<MessageOutputModel>
    {
        public UserImageChangeModel Model { get; set; }

        public ChangeUserImageCommand(UserImageChangeModel request)
        {
            Model = request;
        }
    }
}