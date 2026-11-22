using ArticleProject.Model.Models.CommonModels.InputModels;

namespace ArticleProject.Model.Models.DepartmentModels
{
    public class DepartmentGetOptionsByKeywordInputModel : BaseGetOptionsByKeywordModel
    {
        public long FacultyId { get; set; }
    }
}