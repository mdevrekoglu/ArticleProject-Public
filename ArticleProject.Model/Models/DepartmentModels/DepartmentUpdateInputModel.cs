namespace ArticleProject.Model.Models.DepartmentModels
{
    public class DepartmentUpdateInputModel
    {
        public int Id { get; set; }
        public string DepartmentName { get; set; }
        public int StudentCount { get; set; }
        public int ResearcherCount { get; set; }

        public long FacultyId { get; set; }
    }
}