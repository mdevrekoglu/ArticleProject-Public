namespace ArticleProject.Model.Models.ResearcherModels
{
    public class ResearcherDataModel
    {
        public long Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int? DocumentCount { get; set; }
        public string Position { get; set; }
        public string StaffTitle { get; set; }

        public long AffiliationId { get; set; }
        public string AffiliationName { get; set; }

        public string WoSId { get; set; }
        public string ORCId { get; set; }
        public long ScopusId { get; set; }

        public bool IsActive { get; set; }
    }
}