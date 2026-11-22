using ArticleProject.Model.Models.NotificationModels;
using MediatR;

namespace ArticleProject.Api.Application.Commands.NotificationCommands
{
    public class CreateNotificationCommand : IRequest<NotificationDataOutputModel>
    {
        public NotificationCreateInputModel Model { get; set; }

        public CreateNotificationCommand(NotificationCreateInputModel request)
        {
            Model = request;
        }
    }
}