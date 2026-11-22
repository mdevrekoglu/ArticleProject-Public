using ArticleProject.Api.Application.Commands.JournalCommands;
using ArticleProject.Api.Application.Queries;
using ArticleProject.Api.Attributes;
using ArticleProject.Model.Models.CommonModels.InputModels;
using ArticleProject.Model.Models.JournalModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ArticleProject.Api.Application.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class JournalController : ControllerBase
    {
        private readonly JournalQuery _journalQuery;
        private readonly IMediator _mediator;

        public JournalController(JournalQuery journalQuery, IMediator mediator)
        {
            _journalQuery = journalQuery;
            _mediator = mediator;
        }

        #region GetRequests

        [HttpGet]
        public async Task<IActionResult> GetByIdAsync([FromQuery] KeyInputModel request)
        {
            var result = await _journalQuery.GetByIdAsync(request);
            return Ok(result);
        }

        [HttpGet]
        public IActionResult GetAllPagedBySearchModel([FromQuery] JournalSearchInputModel request)
        {
            var result = _journalQuery.GetAllPagedBySearchModel(request);
            return Ok(result);
        }

        [HttpGet]
        public IActionResult GetOptionsByKeyword([FromQuery] BaseGetOptionsByKeywordModel request)
        {
            var result = _journalQuery.GetOptionsByKeyword(request);
            return Ok(result);
        }

        #endregion GetRequests

        #region PostRequests

        [HttpPost]
        [LogRequest]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateAsync([FromBody] JournalCreateInputModel request)
        {
            var command = new CreateJournalCommand(request);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost]
        [LogRequest]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateAsync([FromBody] JournalUpdateInputModel request)
        {
            var command = new UpdateJournalCommand(request);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost]
        [LogRequest]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UploadExcelAsync([FromForm] BaseFileInputModel request)
        {
            var command = new CreateJournalWithExcelCommand(request);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        #endregion PostRequests
    }
}