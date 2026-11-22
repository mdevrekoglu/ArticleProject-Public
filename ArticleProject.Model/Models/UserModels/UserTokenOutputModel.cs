using ArticleProject.Model.Models.CommonModels.OutputModels;

namespace ArticleProject.Model.Models.UserModels
{
    public class UserTokenOutputModel : MessageOutputModel
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}