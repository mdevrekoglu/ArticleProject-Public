namespace ArticleProject.Model.Models.JournalModels
{
    public class JournalUpdateInputModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string AbbreviationName { get; set; }
        public string ISSN { get; set; }
        public string EISSN { get; set; }
        public decimal? JIF { get; set; }
        public decimal? JIF5Years { get; set; }

        //category fields
        //public long CategoryId { get; set; }

        public string CategoryName { get; set; }
        public int? Quartile { get; set; }
        public int? Rank { get; set; }
        public int? TotalJournals { get; set; }

        //
        public int? Year { get; set; }
    }
}