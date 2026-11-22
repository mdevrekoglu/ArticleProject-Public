using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ArticleProject.Api.Application.Controllers
{
    [ApiController]
    [AllowAnonymous]
    public class RootController : ControllerBase
    {
        [HttpGet("/")]
        public IActionResult GetRoot()
        {
            return Ok(new { Data = true });
        }
    }
}