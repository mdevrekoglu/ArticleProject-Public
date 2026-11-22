namespace ArticleProject.Model.Models.UserModels
{
    public class UserPasswordResetModel
    {
        public string? Email { get; set; }
        public string? NewPassword { get; set; }
        public string? Token { get; set; }
    }
}