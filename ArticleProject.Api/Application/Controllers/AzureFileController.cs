using ArticleProject.Api.Application.Commands.AzureFileCommands;
using ArticleProject.Api.Application.Queries;
using ArticleProject.Api.Attributes;
using ArticleProject.Model.Models.AzureFileModels;
using ArticleProject.Model.Models.AzureFileModels.GenerateReportModels;
using ArticleProject.Model.Models.CommonModels.InputModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ArticleProject.Api.Application.Controllers
{
    [ApiController]
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]/[action]")]
    public class AzureFileController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly AzureFileQuery _azureFileQuery;

        public AzureFileController(IMediator mediator, AzureFileQuery azureFileQuery)
        {
            _mediator = mediator;
            _azureFileQuery = azureFileQuery;
        }

        #region GetRequests

        [HttpGet]
        public IActionResult GetAllPagedBySearchModel([FromQuery] AzureFileSearchInputModel request)
        {
            var result = _azureFileQuery.GetAllPagedBySearchModel(request);
            return Ok(result);
        }

        [HttpGet]
        public async Task<FileResult> DownloadFileByIdAsync([FromQuery] KeyInputModel request)
        {
            var result = await _azureFileQuery.DownloadFileByIdAsync(request);
            return result;
        }

        #endregion GetRequests

        #region PostRequests

        [HttpPost]
        [LogRequest]
        public async Task<IActionResult> UploadFileAsync([FromForm] BaseFileInputModel request)
        {
            var command = new CreateAzureFileCommand(request);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost]
        [LogRequest]
        public async Task<IActionResult> GenerateReportAsync([FromForm] GenerateReportInputModel request)
        {
            var command = new GenerateReportCommand(request);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        #endregion PostRequests
    }
}