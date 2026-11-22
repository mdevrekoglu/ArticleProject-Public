namespace ArticleProject.Model.Models.FacultyModels
{
    public class FacultyDataModel
    {
        public long Id { get; set; }
        public string FacultyName { get; set; }

        public long AffiliationId { get; set; }
        public string AffiliationName { get; set; }
    }
}