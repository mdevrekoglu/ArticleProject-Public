namespace ArticleProject.Model.Models.UserModels
{
    public class UserLogInModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public int UserType { get; set; }
        public bool RememberMe { get; set; }
    }
}