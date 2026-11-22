using ArticleProject.Api.Application.Commands.UserManagementCommands;
using ArticleProject.Api.Application.Queries;
using ArticleProject.Api.Attributes;
using ArticleProject.Model.Models.CommonModels.InputModels;
using ArticleProject.Model.Models.UserManagementModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ArticleProject.Api.Application.Controllers
{
    [ApiController]
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]/[action]")]
    public class UserManagementController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly UserManagementQuery _userManagementQuery;

        public UserManagementController(IMediator mediator, UserManagementQuery userManagementQuery)
        {
            _mediator = mediator;
            _userManagementQuery = userManagementQuery;
        }

        #region GetRequests

        [HttpGet]
        public IActionResult GetAllPagedBySearchModel([FromQuery] UserSearchInputModel request)
        {
            var result = _userManagementQuery.GetAllPagedBySearchModel(request);
            return Ok(result);
        }

        [HttpGet]
        public IActionResult GetByIdAsync([FromQuery] KeyStringInputModel request)
        {
            var result = _userManagementQuery.GetByIdAsync(request);
            return Ok(result);
        }

        [HttpGet]
        public IActionResult GetOptionsByKeyword([FromQuery] BaseGetOptionsByKeywordModel request)
        {
            var result = _userManagementQuery.GetOptionsByKeyword(request);
            return Ok(result);
        }

        [HttpGet]
        public IActionResult GetOptionsBySearchModel([FromQuery] UserOptionsSearchInputModel request)
        {
            var result = _userManagementQuery.GetOptionsBySearchModel(request);
            return Ok(result);
        }

        #endregion GetRequests

        #region PostRequests

        [HttpPost]
        [LogRequest]
        public async Task<IActionResult> AssignUserResearcherAsync([FromBody] UserResearcherAssignInputModel request)
        {
            var command = new AssignUserResearcherCommand(request);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost]
        [LogRequest]
        public async Task<IActionResult> UpdateUserRolesAsync([FromBody] UserRoleUpdateInputModel request)
        {
            var command = new UpdateUserRolesCommand(request);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        #endregion PostRequests
    }
}