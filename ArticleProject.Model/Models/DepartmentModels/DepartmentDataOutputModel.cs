using ArticleProject.Model.Models.CommonModels.OutputModels;

namespace ArticleProject.Model.Models.DepartmentModels
{
    public class DepartmentDataOutputModel : MessageOutputModel
    {
        public long Id { get; set; }
        public string DepartmentName { get; set; }
        public int StudentCount { get; set; }
        public int ResearcherCount { get; set; }

        public long FacultyId { get; set; }
        public string FacultyName { get; set; }

        public long AffiliationId { get; set; }
        public string AffiliationName { get; set; }
    }
}