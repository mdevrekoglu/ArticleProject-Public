using ArticleProject.Api.Application.Commands.FacultyCommands;
using ArticleProject.Api.Application.Queries;
using ArticleProject.Api.Attributes;
using ArticleProject.Model.Models.CommonModels.InputModels;
using ArticleProject.Model.Models.FacultyModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ArticleProject.Api.Application.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class FacultyController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly FacultyQuery _facultyQuery;

        public FacultyController(IMediator mediator, FacultyQuery facultyQuery)
        {
            _mediator = mediator;
            _facultyQuery = facultyQuery;
        }

        #region GetRequests

        [HttpGet]
        public async Task<IActionResult> GetByIdAsync([FromQuery] KeyInputModel request)
        {
            var result = await _facultyQuery.GetByIdAsync(request);
            return Ok(result);
        }

        [HttpGet]
        public IActionResult GetAllPagedBySearchModel([FromQuery] FacultySearchInputModel request)
        {
            var result = _facultyQuery.GetAllPagedBySearchModel(request);
            return Ok(result);
        }

        [HttpGet]
        public IActionResult GetOptionsByKeyword([FromQuery] FacultyGetOptionsByKeywordInputModel request)
        {
            var result = _facultyQuery.GetOptionsByKeyword(request);
            return Ok(result);
        }

        #endregion GetRequests

        #region PostRequests

        [HttpPost]
        [LogRequest]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateAsync([FromBody] FacultyCreateInputModel request)
        {
            var command = new CreateFacultyCommand(request);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost]
        [LogRequest]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateAsync([FromBody] FacultyUpdateInputModel request)
        {
            var command = new UpdateFacultyCommand(request);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        #endregion PostRequests
    }
}