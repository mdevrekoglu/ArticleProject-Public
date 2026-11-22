using Newtonsoft.Json;

namespace ArticleProject.Model.Models.AffiliatonModels.JsonModels
{
    public class AffiliationScopusJsonModel
    {
        [JsonProperty("affiliation-name")]
        public string AffiliationName { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("institution-profile")]
        public InstitutionProfile InstitutionProfile { get; set; }
    }

    public class InstitutionProfile
    {
        private string _affiliationIdStr;
        public long ScopusId { get; private set; }

        [JsonProperty("@affiliation-id")]
        public string AffiliationIdStr
        {
            get => _affiliationIdStr;
            set
            {
                _affiliationIdStr = value;
                ScopusId = long.Parse(value.Trim());
            }
        }

        [JsonProperty("org-type")]
        public string OrgType { get; set; }
    }
}