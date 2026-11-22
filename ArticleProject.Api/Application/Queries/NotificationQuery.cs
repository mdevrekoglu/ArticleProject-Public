using ArticleProject.Api.Application.Services;
using ArticleProject.Domain.Aggregates;
using ArticleProject.Domain.Common.Exceptions;
using ArticleProject.Domain.Common.Extensions;
using ArticleProject.Domain.Common.Interfaces.Query;
using ArticleProject.Domain.Common.Interfaces.Repository;
using ArticleProject.Model.Models.CommonModels.InputModels;
using ArticleProject.Model.Models.NotificationModels;
using ArticleProject.Resources;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace ArticleProject.Api.Application.Queries
{
    public class NotificationQuery : IQuery
    {
        private readonly IReadOnlyRepository<Notification> _notificationRepository;
        private readonly CurrentUserCredentialService _currentUserCredentialServer;
        private readonly IStringLocalizer<NotificationRes> _stringLocalizer;

        public NotificationQuery(IReadOnlyRepository<Notification> notificationRepository, UserManager<User> userManager, CurrentUserCredentialService currentUserCredentialServer,
            IStringLocalizer<NotificationRes> stringLocalizer)
        {
            _notificationRepository = notificationRepository;
            _currentUserCredentialServer = currentUserCredentialServer;
            _stringLocalizer = stringLocalizer;
        }

        public async Task<NotificationUserDataListModel> GetNotificationsPagedAsync(NotificationUserSearchInputModel request)
        {
            var response = new NotificationUserDataListModel();
            var currentUser = _currentUserCredentialServer.User;

            var query = _notificationRepository.GetAllByFilter(p => p.NotificationTargets.Any(t => t.UserId == currentUser.Id), i => i.Include(p => p.NotificationTargets));

            response.Count = await query.CountAsync();
            if (response.Count == 0)
            {
                response.IsSuccessed = false;
                response.Messages.Add(_stringLocalizer["NoNotificationFound"]);
                return response;
            }

            response.Data = query.Select(p => new NotificationUserDataModel
            {
                IsRead = p.NotificationTargets.First(t => t.UserId == currentUser.Id).IsRead,
                Title = p.Title,
                Description = p.Description,
                CreatedDate = p.CreatedDate,
            }).OrderBy(request.GetSortingExpression())
            .Skip(request.PageSize * request.PageIndex)
            .Take(request.PageSize)
            .ToList();

            response.IsSuccessed = true;
            response.Messages.Add(_stringLocalizer["NotificationsFoundSuccessfully"]);
            return response;
        }

        public async Task<int> GetNotificationCountAsync()
        {
            var count = await _notificationRepository.GetAllByFilter(p => p.IsActive && p.NotificationTargets.Any(t => t.UserId == _currentUserCredentialServer.User.Id), i => i.Include(p => p.NotificationTargets)).CountAsync();

            return count;
        }

        public async Task<NotificationDataOutputModel> GetByIdAsync(KeyInputModel request)
        {
            var response = new NotificationDataOutputModel();

            var entity = (await _notificationRepository.GetByIdAsync(request.Id, i => i.Include(p => p.NotificationTargets).ThenInclude(p => p.User))).ThrowIfNull(_stringLocalizer["ErrorNoNotificationRecordFound"]);

            response = new NotificationDataOutputModel
            {
                Id = entity.Id,
                IsActive = entity.IsActive,
                Title = entity.Title,
                Description = entity.Description,
                CreatedDate = entity.CreatedDate,
                Targets = entity.NotificationTargets.Select(p => new NotificationTargetDataModel
                {
                    UserId = p.UserId,
                    IsRead = p.IsRead,
                    Name = string.Concat(p.User.FirstName, " ", p.User.LastName)
                }).ToList(),
            };

            response.Messages.Add(_stringLocalizer["NotificationRecordFoundSuccessfully"]);
            response.IsSuccessed = true;
            return response;
        }

        public NotificationDataListModel GetAllPagedBySearchModel(NotificationSearchInputModel request)
        {
            var response = new NotificationDataListModel();

            var expressionFilter = GetExpressionFilter(request);

            response.Count = _notificationRepository.GetCountByFilter(expressionFilter);
            if (response.Count == 0)
            {
                response.IsSuccessed = false;
                response.Messages.Add(_stringLocalizer["ErrorNoNotificationRecordFound"]);
                return response;
            }

            response.Data = _notificationRepository.GetAllByFilter(expressionFilter, i => i.Include(p => p.NotificationTargets).ThenInclude(p => p.User))
                                                    .Select(p => new NotificationDataModel
                                                    {
                                                        Id = p.Id,
                                                        IsActive = p.IsActive,
                                                        Title = p.Title,
                                                        Description = p.Description,
                                                        CreatedDate = p.CreatedDate,
                                                        Targets = p.NotificationTargets.Select(p => new NotificationTargetDataModel
                                                        {
                                                            UserId = p.UserId,
                                                            IsRead = p.IsRead,
                                                            Name = string.Concat(p.User.FirstName, " ", p.User.LastName)
                                                        }).ToList()
                                                    }).OrderBy(request.GetSortingExpression())
                                                    .Skip(request.PageSize * request.PageIndex)
                                                    .Take(request.PageSize)
                                                    .ToList();

            response.IsSuccessed = true;
            response.Messages.Add(_stringLocalizer["NotificationRecordFoundSuccessfully"]);
            return response;
        }

        private Expression<Func<Notification, bool>> GetExpressionFilter(NotificationSearchInputModel request)
        {
            Expression<Func<Notification, bool>> filter = p => true;

            if (!string.IsNullOrWhiteSpace(request.Title))
            {
                filter = filter.And(p => p.Title.Contains(request.Title));
            }

            if (request.IsActive.HasValue)
            {
                filter = filter.And(p => p.IsActive == request.IsActive.Value);
            }

            if (request.StartDate.HasValue)
            {
                filter = filter.And(p => p.CreatedDate.Date >= request.StartDate.Value.Date);
            }

            if (request.EndDate.HasValue)
            {
                filter = filter.And(p => p.CreatedDate.Date <= request.EndDate.Value.Date);
            }

            return filter;
        }
    }
}