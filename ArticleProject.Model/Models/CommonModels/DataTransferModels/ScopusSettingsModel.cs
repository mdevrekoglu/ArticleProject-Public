namespace ArticleProject.Model.Models.CommonModels.DataTransferModels
{
    public class ScopusSettingsModel
    {
        public string ApiKey { get; set; }
        public string InstitutionToken { get; set; }
        public string BaseLink { get; set; }
        public string ArticleSearchLink { get; set; }
        public string AffiliationSearchLink { get; set; }
        public string AuthorSearchLink { get; set; }
        public string BaseAbstractLink { get; set; }
        public string AbstractCitationCountLink { get; set; }
        public string BaseFullTextLink { get; set; }
        public string SerialTitleLink { get; set; }
    }
}