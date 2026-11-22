using Microsoft.AspNetCore.Http;

namespace ArticleProject.Model.Models.CommonModels.InputModels
{
    public class BaseFileInputModel
    {
        public IFormFile File { get; set; }
    }
}