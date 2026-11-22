using ArticleProject.Model.Models.CommonModels.OutputModels;
using MediatR;

namespace ArticleProject.Api.Application.Commands.AuthenticationCommands
{
    public class RemoveUserImageCommand : IRequest<MessageOutputModel>
    {
    }
}