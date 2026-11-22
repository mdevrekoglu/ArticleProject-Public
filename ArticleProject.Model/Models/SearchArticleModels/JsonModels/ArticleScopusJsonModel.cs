using Newtonsoft.Json;

namespace ArticleProject.Model.Models.SearchArticleModels.JsonModels
{
    public class ArticleScopusJsonModel
    {
        [JsonProperty("opensearch:totalResults")]
        public int TotalResults { get; set; }

        [JsonProperty("opensearch:startIndex")]
        public int StartIndex { get; set; }

        [JsonProperty("opensearch:itemsPerPage")]
        public int ItemsPerPage { get; set; }

        [JsonProperty("entry")]
        public List<ScopusArticleEntry> Entries { get; set; }
    }

    public class ScopusArticleEntry
    {
        private string _identifier;
        public long ScopusId { get; set; }

        [JsonProperty("dc:identifier")]
        public string Identifier
        {
            get => _identifier;
            set
            {
                _identifier = value;
                ScopusId = long.Parse(value.Split(':')[1].Trim());
            }
        }

        [JsonProperty("eid")]
        public string Eid { get; set; }

        [JsonProperty("dc:title")]
        public string Title { get; set; }

        [JsonProperty("dc:creator")]
        public string Creator { get; set; }

        [JsonProperty("prism:publicationName")]
        public string PublicationName { get; set; }

        private string _ISSN;

        [JsonProperty("prism:issn")]
        public string ISSN
        {
            get => _ISSN;
            set
            {
                _ISSN = value.Insert(4, "-");
            }
        }

        private string _EISSN;

        [JsonProperty("prism:eIssn")]
        public string EISSN
        {
            get => _EISSN;
            set
            {
                _EISSN = value.Insert(4, "-");
            }
        }

        [JsonProperty("prism:volume")]
        public string Volume { get; set; }

        [JsonProperty("prism:issueIdentifier")]
        public string Issue { get; set; }

        [JsonProperty("prism:pageRange")]
        public string PageRange { get; set; }

        [JsonProperty("prism:coverDate")]
        public string CoverDate { get; set; }

        [JsonProperty("prism:doi")]
        public string DOI { get; set; }

        [JsonProperty("citedby-count")]
        public int CitedByCount { get; set; }

        [JsonProperty("pubmed-id")]
        public string PubmedId { get; set; }

        [JsonProperty("openaccess")]
        public int OpenAccess { get; set; }

        [JsonProperty("subtypeDescription")]
        public string SubtypeDescription { get; set; }

        [JsonProperty("affiliation")]
        public List<AffiliationInfo> Affiliations { get; set; }
    }

    public class AffiliationInfo
    {
        [JsonProperty("affilname")]
        public string Name { get; set; }

        [JsonProperty("affiliation-city")]
        public string City { get; set; }

        [JsonProperty("affiliation-country")]
        public string Country { get; set; }
    }
}