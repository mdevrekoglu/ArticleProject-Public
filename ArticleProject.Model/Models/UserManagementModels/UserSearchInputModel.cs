using ArticleProject.Model.Models.CommonModels.InputModels;

namespace ArticleProject.Model.Models.UserManagementModels
{
    public class UserSearchInputModel : BaseSearchModel
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public long? AffiliationId { get; set; }
        public long? FacultyId { get; set; }
        public long? DepartmentId { get; set; }
        public string? Role { get; set; }

        public UserSearchInputModel() : base()
        {
            SortingColumn = "firstName";
            SortingType = "asc";
        }
    }
}