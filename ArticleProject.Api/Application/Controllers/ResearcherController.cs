using ArticleProject.Api.Application.Commands.ResearcherCommands;
using ArticleProject.Api.Application.Commands.ScopusServiceCommands;
using ArticleProject.Api.Application.Queries;
using ArticleProject.Api.Attributes;
using ArticleProject.Model.Models.CommonModels.InputModels;
using ArticleProject.Model.Models.ResearcherModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ArticleProject.Api.Application.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ResearcherController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ResearcherQuery _researcherQuery;

        public ResearcherController(ResearcherQuery researcherQuery, IMediator mediator)
        {
            _researcherQuery = researcherQuery;
            _mediator = mediator;
        }

        #region GetRequests

        [HttpGet]
        public async Task<IActionResult> GetByIdAsync([FromQuery] KeyInputModel request)
        {
            var result = await _researcherQuery.GetByIdAsync(request);
            return Ok(result);
        }

        [HttpGet]
        public IActionResult GetAllPagedBySearchModel([FromQuery] ResearcherSearchInputModel request)
        {
            var result = _researcherQuery.GetAllPagedBySearchModel(request);
            return Ok(result);
        }

        [HttpGet]
        public IActionResult GetOptionsByKeyword([FromQuery] BaseGetOptionsByKeywordModel request)
        {
            var result = _researcherQuery.GetOptionsByKeyword(request);
            return Ok(result);
        }

        [HttpGet]
        public IActionResult GetNotAssignedResearcherOptionsByKeyword([FromQuery] BaseGetOptionsByKeywordModel request)
        {
            var result = _researcherQuery.GetNotAssignedResearcherOptionsByKeyword(request);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetResearcherDetailsByIdAsync([FromQuery] KeyInputModel request)
        {
            var result = await _researcherQuery.GetResearcherDetailsByIdAsync(request);
            return Ok(result);
        }

        #endregion GetRequests

        #region PostRequests

        [HttpPost]
        [LogRequest]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> FetchAsync([FromBody] KeyInputModel request)
        {
            var command = new FetchResearcherFromScopusCommand(request);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost]
        [LogRequest]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateAsync([FromBody] ResearcherUpdateInputModel request)
        {
            var command = new UpdateResearcherCommand(request);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        #endregion PostRequests
    }
}