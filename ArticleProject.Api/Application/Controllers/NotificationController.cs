using ArticleProject.Api.Application.Commands.NotificationCommands;
using ArticleProject.Api.Application.Queries;
using ArticleProject.Api.Attributes;
using ArticleProject.Model.Models.CommonModels.InputModels;
using ArticleProject.Model.Models.NotificationModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ArticleProject.Api.Application.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class NotificationController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly NotificationQuery _notificationQuery;

        public NotificationController(IMediator mediator, NotificationQuery notificationQuery)
        {
            _mediator = mediator;
            _notificationQuery = notificationQuery;
        }

        #region GetRequests

        [HttpGet]
        public async Task<IActionResult> GetByIdAsync([FromQuery] KeyInputModel request)
        {
            var result = await _notificationQuery.GetByIdAsync(request);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetNotificationCountAsync()
        {
            var result = await _notificationQuery.GetNotificationCountAsync();
            return Ok(result);
        }

        [HttpGet]
        public IActionResult GetAllPagedBySearchModel([FromQuery] NotificationSearchInputModel request)
        {
            var result = _notificationQuery.GetAllPagedBySearchModel(request);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetNotificationsPagedAsync([FromQuery] NotificationUserSearchInputModel request)
        {
            var result = await _notificationQuery.GetNotificationsPagedAsync(request);
            return Ok(result);
        }

        #endregion GetRequests

        #region PostRequests

        [HttpPost]
        [LogRequest]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateAsync([FromBody] NotificationCreateInputModel request)
        {
            var command = new CreateNotificationCommand(request);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost]
        [LogRequest]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateAsync([FromBody] NotificationUpdateInputModel request)
        {
            var command = new UpdateNotificationCommand(request);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        #endregion PostRequests
    }
}