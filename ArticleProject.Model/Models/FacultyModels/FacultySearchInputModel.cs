using ArticleProject.Model.Models.CommonModels.InputModels;

namespace ArticleProject.Model.Models.FacultyModels
{
    public class FacultySearchInputModel : BaseSearchModel
    {
        public string? FacultyName { get; set; }

        public List<long>? AffiliationIds { get; set; }

        public FacultySearchInputModel() : base()
        {
            SortingColumn = "facultyName";
            SortingType = "asc";
        }
    }
}