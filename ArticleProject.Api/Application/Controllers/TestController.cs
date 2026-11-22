using ArticleProject.Api.Application.Commands.ScopusServiceCommands;
using ArticleProject.Api.Application.Services;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ArticleProject.Api.Application.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class TestController : ControllerBase
    {
        private readonly ScopusService _scopusService;
        private readonly IMediator _mediator;

        public TestController(IMediator mediator, ScopusService scopusService)
        {
            _mediator = mediator;
            _scopusService = scopusService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok();
        }

        [HttpPost]
        public IActionResult Post()
        {
            return Ok();
        }

        [HttpGet]
        [Authorize] // This requires the user to be authenticated
        public IActionResult AuthenticatedTest()
        {
            return Ok("You are authenticated!");
        }

        // Test method to check if the user has the "Lecturer" role
        [HttpGet]
        [Authorize(Roles = "Lecturer")]
        public IActionResult AdminRoleTest()
        {
            return Ok("You are an Admin!");
        }

        // Test method to check if the user has the "Student" role
        [HttpGet]
        [Authorize(Roles = "Student")]
        public IActionResult UserRoleTest()
        {
            return Ok("You are a User!");
        }

        // Test method to check if the user has either "Student" or "Lecturer" role
        [HttpGet]
        [Authorize(Roles = "Student,Lecturer")]
        public IActionResult AnyRoleTest()
        {
            return Ok("You are either an Admin or a User!");
        }

        // Test to get the current user details (JWT token details)
        [HttpGet]
        [Authorize] // Requires user to be authenticated
        public IActionResult GetCurrentUser()
        {
            var userFirstName = User.FindFirst(ClaimTypes.Name)!.Value;
            var userLastName = User.FindFirst(ClaimTypes.Surname)!.Value;
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value; // Gets the user ID from JWT token
            var userEmail = User.FindFirst(ClaimTypes.Email)!.Value; // Gets the user name from the identity
            var userRoles = User.FindAll(ClaimTypes.Role).Select(r => r.Value); // Gets the roles from the JWT token

            return Ok(new
            {
                UserFirstName = userFirstName,
                UserLastName = userLastName,
                UserId = userId,
                UserEmail = userEmail,
                Roles = userRoles
            });
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> TriggerJobCommand()
        {
            var command = new FetchResearchersDataScopusCommand();
            var response = await _mediator.Send(command);
            return Ok(response);
        }
    }
}