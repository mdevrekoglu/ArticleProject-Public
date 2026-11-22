namespace ArticleProject.Model.Models.SearchArticleModels
{
    public class ArticleSearchModel
    {
        public string? Title { get; set; }
        public long? AffiliationScopusId { get; set; }
        public int? PublishYear { get; set; }
        public string? DOI { get; set; }
        public List<long>? ResearcherScopusIds { get; set; }
        public List<string>? JournalIssnIds { get; set; }
        public List<string>? JournalEissnIds { get; set; }

        public int Index { get; set; }
        public int Count { get; set; }
    }
}