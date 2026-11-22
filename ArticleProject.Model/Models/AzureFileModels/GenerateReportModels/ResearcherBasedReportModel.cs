namespace ArticleProject.Model.Models.AzureFileModels.GenerateReportModels
{
    public class ResearcherBasedReportModel
    {
        public long ResearcherId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Position { get; set; }
        public string StaffTitle { get; set; }

        public long ScopusId { get; set; }
        public string WoSId { get; set; }
        public string ORCId { get; set; }

        public List<PublicationsByYear> Publications { get; set; }
        public int TotalPublicationsCount => Publications?.Sum(p => p.TotalCount) ?? 0;
    }

    public class PublicationsByYear
    {
        public int Year { get; set; }
        public int Q1Count { get; set; }
        public int Q2Count { get; set; }
        public int Q3Count { get; set; }
        public int Q4Count { get; set; }
        public int NotCategorizedCount { get; set; }
        public int TotalCount => Q1Count + Q2Count + Q3Count + Q4Count + NotCategorizedCount;
    }
}