using ArticleProject.Model.Models.NotificationModels;
using MediatR;

namespace ArticleProject.Api.Application.Commands.NotificationCommands
{
    public class UpdateNotificationCommand : IRequest<NotificationDataOutputModel>
    {
        public NotificationUpdateInputModel Model { get; set; }

        public UpdateNotificationCommand(NotificationUpdateInputModel request)
        {
            Model = request;
        }
    }
}