using Newtonsoft.Json;

namespace ArticleProject.Model.Models.ResearcherModels.JsonModels
{
    public class ResearcherScopusJsonModel
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

        private string _orcid;

        [JsonProperty("orcid")]
        public string Orcid
        {
            get => _orcid;
            set
            {
                _orcid = (!string.IsNullOrEmpty(value) && value.Length > 2) ? value.Substring(1, value.Length - 2) : null; // Remove []
            }
        }

        [JsonProperty("preferred-name")]
        public PreferredName PreferredName { get; set; }

        [JsonProperty("document-count")]
        public int DocumentCount { get; set; }

        [JsonProperty("affiliation-current")]
        public AffiliationCurrent AffiliationCurrent { get; set; }
    }

    public class PreferredName
    {
        [JsonProperty("surname")]
        public string Surname { get; set; }

        [JsonProperty("given-name")]
        public string GivenName { get; set; }

        //[JsonProperty("initials")]
        //public string Initials { get; set; }
    }

    public class AffiliationCurrent
    {
        [JsonProperty("affiliation-id")]
        public long AffiliationId { get; set; }
    }
}