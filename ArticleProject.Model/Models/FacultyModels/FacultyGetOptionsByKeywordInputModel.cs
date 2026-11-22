using ArticleProject.Model.Models.CommonModels.InputModels;

namespace ArticleProject.Model.Models.FacultyModels
{
    public class FacultyGetOptionsByKeywordInputModel : BaseGetOptionsByKeywordModel
    {
        public long AffiliationId { get; set; }
    }
}