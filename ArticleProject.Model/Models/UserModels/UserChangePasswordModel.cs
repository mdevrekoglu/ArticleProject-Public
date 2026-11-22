namespace ArticleProject.Model.Models.UserModels
{
    public class UserChangePasswordModel
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? NewPassword { get; set; }
    }
}