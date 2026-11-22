using ArticleProject.Model.Models.CommonModels.OutputModels;

namespace ArticleProject.Model.Models.MajorDisciplineModels
{
    public class MajorDisciplineDataOutputModel : MessageOutputModel
    {
        public long Id { get; set; }
        public string MajorDisciplineName { get; set; }

        public long DepartmentId { get; set; }
        public string DepartmentName { get; set; }

        public long FacultyId { get; set; }
        public string FacultyName { get; set; }

        public long AffiliationId { get; set; }
        public string AffiliationName { get; set; }
    }
}