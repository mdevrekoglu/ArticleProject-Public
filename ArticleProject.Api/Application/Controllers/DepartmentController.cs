using ArticleProject.Api.Application.Commands.DepartmentCommands;
using ArticleProject.Api.Application.Queries;
using ArticleProject.Api.Attributes;
using ArticleProject.Model.Models.CommonModels.InputModels;
using ArticleProject.Model.Models.DepartmentModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ArticleProject.Api.Application.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class DepartmentController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly DepartmentQuery _departmentQuery;

        public DepartmentController(IMediator mediator, DepartmentQuery departmentQuery)
        {
            _mediator = mediator;
            _departmentQuery = departmentQuery;
        }

        #region GetRequests

        [HttpGet]
        public async Task<IActionResult> GetByIdAsync([FromQuery] KeyInputModel request)
        {
            var result = await _departmentQuery.GetByIdAsync(request);
            return Ok(result);
        }

        [HttpGet]
        public IActionResult GetAllPagedBySearchModel([FromQuery] DepartmentSearchInputModel request)
        {
            var result = _departmentQuery.GetAllPagedBySearchModel(request);
            return Ok(result);
        }

        [HttpGet]
        public IActionResult GetOptionsByKeyword([FromQuery] DepartmentGetOptionsByKeywordInputModel request)
        {
            var result = _departmentQuery.GetOptionsByKeyword(request);
            return Ok(result);
        }

        #endregion GetRequests

        #region PostRequests

        [HttpPost]
        [LogRequest]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateAsync([FromBody] DepartmentCreateInputModel request)
        {
            var command = new CreateDepartmentCommand(request);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost]
        [LogRequest]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateAsync([FromBody] DepartmentUpdateInputModel request)
        {
            var command = new UpdateDepartmentCommand(request);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        #endregion PostRequests
    }
}