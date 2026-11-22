namespace ArticleProject.Model.Models.UserModels
{
    public class UserRegisterModel
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public int UserType { get; set; }
    }
}