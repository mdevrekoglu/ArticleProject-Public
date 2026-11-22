using ArticleProject.Model.Constants;

namespace ArticleProject.Model.Models.UserManagementModels
{
    public class UserOptionsSearchInputModel
    {
        public long? AffiliationId { get; set; }
        public long? FacultyId { get; set; }
        public long? DepartmentId { get; set; }
        public int OptionsSize { get; set; } = QuerySizeConstants.OptionsLimit;
    }
}