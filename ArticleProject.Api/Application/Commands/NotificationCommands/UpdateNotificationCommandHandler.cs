using ArticleProject.Domain.Aggregates;
using ArticleProject.Domain.Common.Exceptions;
using ArticleProject.Domain.Common.Interfaces.Repository;
using ArticleProject.Model.Models.NotificationModels;
using ArticleProject.Resources;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace ArticleProject.Api.Application.Commands.NotificationCommands
{
    public class UpdateNotificationCommandHandler : IRequestHandler<UpdateNotificationCommand, NotificationDataOutputModel>
    {
        private readonly IRepository<Notification> _notificationRepository;
        private readonly IRepository<NotificationTarget> _notificationTargetRepository;
        private readonly IStringLocalizer<NotificationRes> _stringLocalizer;

        public UpdateNotificationCommandHandler(IRepository<Notification> notificationRepository, IRepository<NotificationTarget> notificationTargetRepository, IStringLocalizer<NotificationRes> stringLocalizer)
        {
            _notificationRepository = notificationRepository;
            _notificationTargetRepository = notificationTargetRepository;
            _stringLocalizer = stringLocalizer;
        }

        public async Task<NotificationDataOutputModel> Handle(UpdateNotificationCommand request, CancellationToken cancellationToken)
        {
            var notification = (await _notificationRepository.GetByIdAsync(request.Model.Id,
                                    i => i.Include(p => p.NotificationTargets).ThenInclude(p => p.User)))
                                    .ThrowIfNull(_stringLocalizer["ErrorNoNotificationRecordFound"]);

            // Check if the notification columns have changed
            if (HasChanged(notification, request.Model))
                notification.Update(request.Model.Title, request.Model.Description, request.Model.IsActive);

            // Delete user targets
            var deletedNotificationTargets = notification.NotificationTargets.Where(p => !request.Model.UserIds.Contains(p.UserId)).ToList();
            if (deletedNotificationTargets != null && deletedNotificationTargets.Any())
                _notificationTargetRepository.DeleteRange(deletedNotificationTargets);

            // Add new user targets
            var userIds = notification.NotificationTargets.Select(p => p.UserId).ToList();
            var newNotificationTargets = request.Model.UserIds.Where(p => !userIds.Contains(p)).Select(p => new NotificationTarget(p)).ToList();
            if (newNotificationTargets != null && newNotificationTargets.Any())
                notification.AddRangeNotificationTarget(newNotificationTargets);

            await _notificationRepository.SaveChangesAsync();

            var updatedNotification = await _notificationRepository.GetByIdAsync(notification.Id, i => i.Include(p => p.NotificationTargets).ThenInclude(p => p.User));

            var response = new NotificationDataOutputModel
            {
                Id = updatedNotification.Id,
                Title = updatedNotification.Title,
                CreatedDate = updatedNotification.CreatedDate,
                Description = updatedNotification.Description,
                IsActive = updatedNotification.IsActive,
                Targets = updatedNotification.NotificationTargets.Select(p => new NotificationTargetDataModel
                {
                    IsRead = true,
                    UserId = p.UserId,
                    Name = string.Concat(p.User.FirstName, " ", p.User.LastName)
                }).ToList(),
            };
            response.IsSuccessed = true;
            response.Messages.Add(_stringLocalizer["NotificationUpdatedSuccessfully"]);
            return response;
        }

        private bool HasChanged(Notification notification, NotificationUpdateInputModel model)
        {
            return notification.Title != model.Title || notification.Description != model.Description || notification.IsActive != model.IsActive;
        }
    }
}