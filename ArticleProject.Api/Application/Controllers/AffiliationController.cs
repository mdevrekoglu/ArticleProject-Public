using ArticleProject.Api.Application.Commands.AffiliationCommands;
using ArticleProject.Api.Application.Commands.ScopusServiceCommands;
using ArticleProject.Api.Application.Queries;
using ArticleProject.Api.Attributes;
using ArticleProject.Model.Models.AffiliatonModels;
using ArticleProject.Model.Models.CommonModels.InputModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ArticleProject.Api.Application.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AffiliationController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly AffiliationQuery _affiliationQuery;

        public AffiliationController(IMediator mediator, AffiliationQuery affiliationQuery)
        {
            _mediator = mediator;
            _affiliationQuery = affiliationQuery;
        }

        #region GetRequests

        [HttpGet]
        public async Task<IActionResult> GetByIdAsync([FromQuery] KeyInputModel request)
        {
            var result = await _affiliationQuery.GetByIdAsync(request);
            return Ok(result);
        }

        [HttpGet]
        public IActionResult GetAllPagedBySearchModel([FromQuery] AffiliationSearchInputModel request)
        {
            var result = _affiliationQuery.GetAllPagedBySearchModel(request);
            return Ok(result);
        }

        [HttpGet]
        public IActionResult GetOptionsByKeyword([FromQuery] BaseGetOptionsByKeywordModel request)
        {
            var result = _affiliationQuery.GetOptionsByKeyword(request);
            return Ok(result);
        }

        #endregion GetRequests

        #region PostRequests

        [HttpPost]
        [LogRequest]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateAsync([FromBody] AffiliationCreateInputModel request)
        {
            var command = new CreateAffiliationCommand(request);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost]
        [LogRequest]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateAsync([FromBody] AffiliationUpdateInputModel request)
        {
            var command = new UpdateAffiliationCommand(request);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost]
        [LogRequest]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> FetchAsync([FromBody] KeyInputModel request)
        {
            var command = new FetchAffiliationFromScopusCommand(request);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        #endregion PostRequests
    }
}