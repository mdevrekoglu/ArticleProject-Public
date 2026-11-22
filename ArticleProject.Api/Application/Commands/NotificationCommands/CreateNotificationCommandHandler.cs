using ArticleProject.Api.Application.Services;
using ArticleProject.Domain.Aggregates;
using ArticleProject.Domain.Common.Interfaces.Repository;
using ArticleProject.Model.Models.NotificationModels;
using ArticleProject.Resources;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace ArticleProject.Api.Application.Commands.NotificationCommands
{
    public class CreateNotificationCommandHandler : IRequestHandler<CreateNotificationCommand, NotificationDataOutputModel>
    {
        private readonly IRepository<Notification> _notificationRepository;
        private readonly IStringLocalizer<NotificationRes> _stringLocalizer;
        private readonly CurrentUserCredentialService _currentUserCredentialService;

        public CreateNotificationCommandHandler(IRepository<Notification> notificationRepository, IStringLocalizer<NotificationRes> stringLocalizer,
                                                CurrentUserCredentialService currentUserCredentialService)
        {
            _notificationRepository = notificationRepository;
            _stringLocalizer = stringLocalizer;
            _currentUserCredentialService = currentUserCredentialService;
        }

        public async Task<NotificationDataOutputModel> Handle(CreateNotificationCommand request, CancellationToken cancellationToken)
        {
            var newEntity = new Notification(request.Model.Title, request.Model.Description, _currentUserCredentialService.User.Id, request.Model.UserIds.Select(p => new NotificationTarget(p)).ToList());

            await _notificationRepository.AddAndSaveAsync(newEntity);

            var entity = await _notificationRepository.GetByIdAsync(newEntity.Id, i => i.Include(p => p.NotificationTargets).ThenInclude(p => p.User));

            var response = new NotificationDataOutputModel
            {
                Id = entity.Id,
                Title = entity.Title,
                Description = entity.Description,
                CreatedDate = entity.CreatedDate,
                IsActive = entity.IsActive,
                Targets = entity.NotificationTargets.Select(p => new NotificationTargetDataModel
                {
                    UserId = p.UserId,
                    Name = string.Concat(p.User.FirstName, " ", p.User.LastName),
                    IsRead = p.IsRead,
                }).ToList(),
            };

            response.IsSuccessed = true;
            response.Messages.Add(_stringLocalizer["NotificationCreatedSuccessfully"]);
            return response;
        }
    }
}