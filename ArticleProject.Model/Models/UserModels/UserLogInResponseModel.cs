using ArticleProject.Model.Models.CommonModels.OutputModels;

namespace ArticleProject.Model.Models.UserModels
{
    public class UserLogInResponseModel : MessageOutputModel
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }

        public UserLogInResponseModel() : base()
        { }
    }
}