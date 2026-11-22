namespace ArticleProject.Model.Models.UserModels
{
    // Used on reset password and email confirm operations
    public class UserRequestModel
    {
        public string? Email { get; set; }
        public int UserType { get; set; }
    }
}