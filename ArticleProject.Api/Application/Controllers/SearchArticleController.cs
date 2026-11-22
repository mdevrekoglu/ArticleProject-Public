using ArticleProject.Api.Application.Queries;
using ArticleProject.Model.Models.SearchArticleModels;
using Microsoft.AspNetCore.Mvc;

namespace ArticleProject.Api.Application.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class SearchArticleController : ControllerBase
    {
        private readonly SearchArticleQuery _searchArticleQuery;

        public SearchArticleController(SearchArticleQuery searchArticleQuery)
        {
            _searchArticleQuery = searchArticleQuery;
        }

        #region GetRequests

        [HttpGet]
        public async Task<IActionResult> GetAllPagedBySearchModelAsync([FromQuery] ArticleSearchInputModel request)
        {
            var result = await _searchArticleQuery.GetAllPagedBySearchModelAsync(request);
            return Ok(result);
        }

        #endregion GetRequests
    }
}