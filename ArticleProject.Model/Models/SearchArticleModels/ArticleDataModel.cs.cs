namespace ArticleProject.Model.Models.SearchArticleModels
{
    public class ArticleDataModel
    {
        public long ScopusId { get; set; }
        public string Eid { get; set; }
        public string Title { get; set; }
        public string Creator { get; set; }
        public string PublicationName { get; set; }
        public string ISSN { get; set; }
        public string EISSN { get; set; }
        public string Volume { get; set; }
        public string Issue { get; set; }
        public string PageRange { get; set; }
        public string CoverDate { get; set; }
        public string DOI { get; set; }
        public int CitedByCount { get; set; }
        public string AffiliationName { get; set; }

        public long? JournalId { get; set; }
        public string JournalName { get; set; }
        public string JournalAbbreviationName { get; set; }
        public string CategoryName { get; set; }
    }
}