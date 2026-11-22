using ArticleProject.Model.Models.CommonModels.InputModels;

namespace ArticleProject.Model.Models.DepartmentModels
{
    public class DepartmentSearchInputModel : BaseSearchModel
    {
        public string? DepartmentName { get; set; }
        public long? AffiliationId { get; set; }
        public long? FacultyId { get; set; }

        public DepartmentSearchInputModel() : base()
        {
            SortingColumn = "departmentName";
            SortingType = "asc";
        }
    }
}