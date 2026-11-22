namespace ArticleProject.Model.Models.DepartmentModels
{
    public class DepartmentCreateInputModel
    {
        public string DepartmentName { get; set; }
        public int StudentCount { get; set; }
        public int ResearcherCount { get; set; }

        public long FacultyId { get; set; }
    }
}