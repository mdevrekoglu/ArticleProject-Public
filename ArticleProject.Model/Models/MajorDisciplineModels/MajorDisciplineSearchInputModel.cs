using ArticleProject.Model.Models.CommonModels.InputModels;

namespace ArticleProject.Model.Models.MajorDisciplineModels
{
    public class MajorDisciplineSearchInputModel : BaseSearchModel
    {
        public string? MajorDisciplineName { get; set; }

        public List<long>? DepartmentIds { get; set; }
        public List<long>? FacultyIds { get; set; }
        public List<long>? AffiliationIds { get; set; }

        public MajorDisciplineSearchInputModel() : base()
        {
            SortingColumn = "majorDisciplineName";
            SortingType = "asc";
        }
    }
}