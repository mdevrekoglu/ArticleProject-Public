using Newtonsoft.Json;

namespace ArticleProject.Model.Models.ResearcherModels.JsonModels
{
    public class ORCExternalIdentifierJsonModel
    {
        [JsonProperty("created-date")]
        public DateValue CreatedDate { get; set; }

        [JsonProperty("last-modified-date")]
        public DateValue LastModifiedDate { get; set; }

        [JsonProperty("source")]
        public SourceInfo Source { get; set; }

        [JsonProperty("external-id-type")]
        public string ExternalIdType { get; set; }

        [JsonProperty("external-id-value")]
        public string ExternalIdValue { get; set; }

        [JsonProperty("external-id-url")]
        public UrlValue ExternalIdUrl { get; set; }

        [JsonProperty("external-id-relationship")]
        public string ExternalIdRelationship { get; set; }

        [JsonProperty("visibility")]
        public string Visibility { get; set; }

        [JsonProperty("path")]
        public string Path { get; set; }

        [JsonProperty("put-code")]
        public int PutCode { get; set; }

        [JsonProperty("display-index")]
        public int DisplayIndex { get; set; }
    }

    public class DateValue
    {
        [JsonProperty("value")]
        public long Value { get; set; }
    }

    public class SourceInfo
    {
        [JsonProperty("source-orcid")]
        public OrcidId SourceOrcid { get; set; }  // nullable in example

        [JsonProperty("source-client-id")]
        public ClientId SourceClientId { get; set; }

        [JsonProperty("source-name")]
        public ValueString SourceName { get; set; }

        [JsonProperty("assertion-origin-orcid")]
        public OrcidId AssertionOriginOrcid { get; set; }  // nullable

        [JsonProperty("assertion-origin-client-id")]
        public ClientId AssertionOriginClientId { get; set; } // nullable

        [JsonProperty("assertion-origin-name")]
        public ValueString AssertionOriginName { get; set; }
    }

    public class OrcidId
    {
        [JsonProperty("uri")]
        public string Uri { get; set; }

        [JsonProperty("path")]
        public string Path { get; set; }

        [JsonProperty("host")]
        public string Host { get; set; }
    }

    public class ClientId
    {
        [JsonProperty("uri")]
        public string Uri { get; set; }

        [JsonProperty("path")]
        public string Path { get; set; }

        [JsonProperty("host")]
        public string Host { get; set; }
    }

    public class ValueString
    {
        [JsonProperty("value")]
        public string Value { get; set; }
    }

    public class UrlValue
    {
        [JsonProperty("value")]
        public string Value { get; set; }
    }
}