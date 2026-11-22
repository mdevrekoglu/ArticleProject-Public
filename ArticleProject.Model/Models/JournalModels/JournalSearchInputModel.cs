using ArticleProject.Model.Models.CommonModels.InputModels;

namespace ArticleProject.Model.Models.JournalModels
{
    public class JournalSearchInputModel : BaseSearchModel
    {
        public string? Name { get; set; }
        public string? AbbreviationName { get; set; }
        public string? ISSN { get; set; }
        public string? EISSN { get; set; }
        public string? CategoryName { get; set; }
        public int? Quartile { get; set; }
        public int? Rank { get; set; }
        public int? TotalJournals { get; set; }
        public int? Year { get; set; }
        public long? CategoryId { get; set; }

        public JournalSearchInputModel() : base()
        {
            SortingColumn = "name";
            SortingType = "asc";
        }
    }
}