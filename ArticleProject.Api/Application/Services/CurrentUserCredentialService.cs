using ArticleProject.Domain.Common.Interfaces.Service;
using ArticleProject.Model.Models.CommonModels.DataTransferModels;
using System.Security.Claims;

namespace ArticleProject.Api.Application.Services
{
    public class CurrentUserCredentialService : IBaseScopedService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserCredentialService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public CurrentUserCredential User => GetCurrentUser();

        public bool IsAuthenticated => _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;

        private CurrentUserCredential GetCurrentUser()
        {
            var user = _httpContextAccessor.HttpContext?.User;

            if (user == null || !user.Identity.IsAuthenticated)
                return null;

            return new CurrentUserCredential
            {
                Id = user.FindFirstValue(ClaimTypes.NameIdentifier)!,
                Email = user.FindFirstValue(ClaimTypes.Email)!,
                FirstName = user.FindFirstValue(ClaimTypes.Name)!,
                LastName = user.FindFirstValue(ClaimTypes.Surname)!
            };
        }
    }
}