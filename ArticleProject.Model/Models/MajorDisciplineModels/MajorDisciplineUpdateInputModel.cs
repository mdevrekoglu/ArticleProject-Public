namespace ArticleProject.Model.Models.MajorDisciplineModels
{
    public class MajorDisciplineUpdateInputModel
    {
        public long Id { get; set; }
        public string MajorDisciplineName { get; set; }

        public long DepartmentId { get; set; }
    }
}