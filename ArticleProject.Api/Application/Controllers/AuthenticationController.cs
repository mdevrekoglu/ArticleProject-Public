using ArticleProject.Api.Application.Commands.AuthenticationCommands;
using ArticleProject.Api.Application.Queries;
using ArticleProject.Api.Attributes;
using ArticleProject.Model.Models.UserModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ArticleProject.Api.Application.Controllers
{
    [LogRequest]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly AuthenticationQuery _authenticationQuery;

        public AuthenticationController(IMediator mediator, AuthenticationQuery authenticationQuery)
        {
            _mediator = mediator;
            _authenticationQuery = authenticationQuery;
        }

        #region GetRequests

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ResendEmailConfirm([FromQuery] UserRequestModel request)
        {
            var result = await _authenticationQuery.ResendEmailConfirm(request);
            return Ok(result);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPasswordEmail([FromQuery] UserRequestModel request)
        {
            var result = await _authenticationQuery.ResetPasswordEmail(request);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetUserCredentialsAsync()
        {
            var result = await _authenticationQuery.GetUserCredentialsAsync();
            return Ok(result);
        }

        #endregion GetRequests

        #region PostRequests

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] UserRegisterModel request)
        {
            var command = new RegisterCommand(request);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] UserLogInModel request)
        {
            var result = await _authenticationQuery.LogIn(request);
            return Ok(result);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken([FromBody] UserTokenInputModel request)
        {
            var result = await _authenticationQuery.RefreshToken(request);
            return Ok(result);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail([FromBody] UserConfirmEmailModel request)
        {
            var command = new EmailConfirmCommand(request);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromBody] UserPasswordResetModel request)
        {
            var command = new ResetPasswordCommand(request);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword([FromBody] UserChangePasswordModel request)
        {
            var command = new ChangePasswordCommand(request);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeUserImageAsync([FromBody] UserImageChangeModel request)
        {
            var command = new ChangeUserImageCommand(request);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> RemoveUserImageAsync()
        {
            var command = new RemoveUserImageCommand();
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        #endregion PostRequests
    }
}