namespace ArticleProject.Model.Models.AzureFileModels.GenerateReportModels
{
    public class GenerateReportExcelDataModel
    {
        public int RowNumber { get; set; }

        public string PublicationType { get; set; }
        public string Authors { get; set; }
        public string AuthorFullNames { get; set; }
        public string ArticleTitle { get; set; }
        public string SourceTitle { get; set; }
        public string DocumentType { get; set; }
        public string Affiliations { get; set; }
        public string WosIds { get; set; }
        public string OrcIDs { get; set; }
        public decimal? CitedReferenceCount { get; set; }
        public decimal? TimesCitedWoSCore { get; set; }
        public decimal? TimesCitedAllDatabases { get; set; }
        public string Publisher { get; set; }
        public string ISSN { get; set; }
        public string EISSN { get; set; }
        public string PublicationDate { get; set; }
        public decimal? PublicationYear { get; set; }
        public string DOI { get; set; }
        public string WosCategories { get; set; }
        public string ResearchAreas { get; set; }
        public string UniqueWOSID { get; set; }
    }
}