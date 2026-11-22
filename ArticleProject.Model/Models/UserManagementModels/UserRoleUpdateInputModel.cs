namespace ArticleProject.Model.Models.UserManagementModels
{
    public class UserRoleUpdateInputModel
    {
        public string UserId { get; set; }
        public List<string> RoleIds { get; set; }
    }
}