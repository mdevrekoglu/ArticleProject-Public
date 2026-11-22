using ArticleProject.Model.Models.CommonModels.OutputModels;

namespace ArticleProject.Model.Models.UserModels
{
    public class UserCredentialsModel : MessageOutputModel
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Image { get; set; }
        public string DepartmentName { get; set; }
        public string FacultyName { get; set; }
        public string AffiliationName { get; set; }
    }
}