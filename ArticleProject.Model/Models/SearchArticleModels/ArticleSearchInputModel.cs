using ArticleProject.Model.Models.CommonModels.InputModels;

namespace ArticleProject.Model.Models.SearchArticleModels
{
    public class ArticleSearchInputModel : BaseSearchModel
    {
        public string? Title { get; set; }
        public long? AffiliationId { get; set; }
        public int? PublishYear { get; set; }
        public string? DOI { get; set; }
        public List<long>? ResearcherIds { get; set; }
        public List<long>? JournalIds { get; set; }

        public ArticleSearchInputModel() : base()
        {
            SortingColumn = "";
            SortingType = "";
        }
    }
}