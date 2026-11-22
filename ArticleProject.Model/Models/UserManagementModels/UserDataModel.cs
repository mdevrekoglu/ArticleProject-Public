namespace ArticleProject.Model.Models.UserManagementModels
{
    public class UserDataModel
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public long? DepartmentId { get; set; }
        public string? DepartmentName { get; set; }

        public string? FacultyName { get; set; }

        public string? AffiliationName { get; set; }

        public long? ResearcherId { get; set; }

        public List<string> Roles { get; set; }
    }
}