using ArticleProject.Api.Application.Commands.MajorDisciplineCommands;
using ArticleProject.Api.Application.Queries;
using ArticleProject.Model.Models.CommonModels.InputModels;
using ArticleProject.Model.Models.MajorDisciplineModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ArticleProject.Api.Application.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class MajorDisciplineController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly MajorDisciplineQuery _majorDisciplineQuery;

        public MajorDisciplineController(IMediator mediator, MajorDisciplineQuery majorDisciplineQuery)
        {
            _mediator = mediator;
            _majorDisciplineQuery = majorDisciplineQuery;
        }

        #region GetRequests

        [HttpGet]
        public async Task<IActionResult> GetByIdAsync([FromQuery] KeyInputModel request)
        {
            try
            {
                var result = await _majorDisciplineQuery.GetByIdAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet]
        public IActionResult GetAllPagedBySearchModel([FromQuery] MajorDisciplineSearchInputModel request)
        {
            try
            {
                var result = _majorDisciplineQuery.GetAllPagedBySearchModel(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        #endregion GetRequests

        #region PostRequests

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateAsync([FromBody] MajorDisciplineCreateInputModel request)
        {
            try
            {
                var command = new CreateMajorDisciplineCommand(request);
                var result = await _mediator.Send(command);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateAsync([FromBody] MajorDisciplineUpdateInputModel request)
        {
            try
            {
                var command = new UpdateMajorDisciplineCommand(request);
                var result = await _mediator.Send(command);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        #endregion PostRequests
    }
}