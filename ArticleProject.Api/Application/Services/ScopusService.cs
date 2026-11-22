using ArticleProject.Domain.Common.Interfaces.Service;
using ArticleProject.Model.Models.AffiliatonModels.JsonModels;
using ArticleProject.Model.Models.CommonModels.DataTransferModels;
using ArticleProject.Model.Models.ResearcherModels.JsonModels;
using ArticleProject.Model.Models.SearchArticleModels;
using ArticleProject.Model.Models.SearchArticleModels.JsonModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ArticleProject.Api.Application.Services
{
    public class ScopusService : IBaseSingletonService
    {
        private readonly HttpClient _httpClient;
        private readonly ScopusSettingsModel _scopusSettings;

        public ScopusService(IConfiguration configuration)
        {
            _scopusSettings = configuration.GetSection("ScopusSettings").Get<ScopusSettingsModel>()!; // Not null

            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(_scopusSettings.BaseLink)
            };

            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            _httpClient.DefaultRequestHeaders.Add("X-ELS-APIKey", _scopusSettings.ApiKey);
            _httpClient.DefaultRequestHeaders.Add("X-ELS-Insttoken", _scopusSettings.InstitutionToken);
        }

        public async Task<List<ResearcherScopusJsonModel>> GetResearchersAsync(long affiliationScopusId, int index, int count)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, $"{_scopusSettings.AuthorSearchLink}" +
                                                                                $"?query=AF-ID({affiliationScopusId})" +
                                                                                $"&start={index}" +
                                                                                $"&count={count}");

            HttpResponseMessage response = await _httpClient.SendAsync(request);
            JObject json = JObject.Parse(await response.Content.ReadAsStringAsync());

            try
            {
                return JsonConvert.DeserializeObject<List<ResearcherScopusJsonModel>>(json["search-results"]["entry"].ToString());
            }
            catch
            {
                return null;
            }
        }

        public async Task<int?> GetResearcherCountAsync(long affiliationScopusId)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, $"{_scopusSettings.AuthorSearchLink}" +
                                                                                $"?query=AF-ID({affiliationScopusId})" +
                                                                                $"&start={0}" +
                                                                                $"&count={1}");

            HttpResponseMessage response = await _httpClient.SendAsync(request);
            JObject json = JObject.Parse(await response.Content.ReadAsStringAsync());

            try
            {
                return int.Parse((string)json["search-results"]["opensearch:totalResults"]);
            }
            catch
            {
                return null;
            }
        }

        public async Task<ResearcherScopusJsonModel> GetResearcherByScopusIdAsync(long scopusId)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, $"{_scopusSettings.AuthorSearchLink}" +
                                                                                $"?query=AU-ID({scopusId})" +
                                                                                $"&start={0}" +
                                                                                $"&count={1}");

            HttpResponseMessage response = await _httpClient.SendAsync(request);
            JObject json = JObject.Parse(await response.Content.ReadAsStringAsync());

            try
            {
                return JsonConvert.DeserializeObject<List<ResearcherScopusJsonModel>>(json["search-results"]["entry"].ToString()).FirstOrDefault();
            }
            catch
            {
                return null;
            }
        }

        public async Task<AffiliationScopusJsonModel> GetAffiliationByScopusIdAsync(long scopusId)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, $"{_scopusSettings.AffiliationSearchLink}" +
                                                                                $"/{scopusId}" +
                                                                                $"?start={0}" +
                                                                                $"&count={1}");

            HttpResponseMessage response = await _httpClient.SendAsync(request);
            JObject json = JObject.Parse(await response.Content.ReadAsStringAsync());

            try
            {
                return JsonConvert.DeserializeObject<AffiliationScopusJsonModel>(json["affiliation-retrieval-response"].ToString());
            }
            catch
            {
                return null;
            }
        }

        public async Task<ArticleScopusJsonModel> GetArticlesByFilterAsync(ArticleSearchModel filter)
        {
            var queryParts = new List<string>();
            if (!string.IsNullOrEmpty(filter.DOI))
            {
                queryParts.Add($"DOI({filter.DOI})");
            }
            else
            {
                if (!string.IsNullOrEmpty(filter.Title))
                    queryParts.Add($"TITLE({filter.Title})");

                if (filter.PublishYear != null)
                    queryParts.Add($"PUBYEAR IS {filter.PublishYear}");

                if (filter.AffiliationScopusId != null)
                    queryParts.Add($"AF-ID({filter.AffiliationScopusId})");

                if (filter.ResearcherScopusIds != null && filter.ResearcherScopusIds.Any())
                    queryParts.Add($"({string.Join(" OR ", filter.ResearcherScopusIds.Select(id => $"AU-ID({id})"))})");

                if (filter.JournalIssnIds != null && filter.JournalIssnIds.Any())
                    queryParts.Add($"({string.Join(" OR ", filter.JournalIssnIds.Select(id => $"ISSN({id})"))})");

                if (filter.JournalEissnIds != null && filter.JournalEissnIds.Any())
                    queryParts.Add($"({string.Join(" OR ", filter.JournalEissnIds.Select(id => $"EISSN({id})"))})");
            }

            var query = string.Join(" AND ", queryParts);

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, $"{_scopusSettings.ArticleSearchLink}?start={filter.Index}&count={filter.Count}&query={query}");

            HttpResponseMessage response = await _httpClient.SendAsync(request);
            JObject json = JObject.Parse(await response.Content.ReadAsStringAsync());

            try
            {
                return JsonConvert.DeserializeObject<ArticleScopusJsonModel>(json["search-results"].ToString());
            }
            catch
            {
                return null;
            }
        }

        public async Task<int?> GetArticleCountAsync(long researcherScopusId)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, $"{_scopusSettings.ArticleSearchLink}" +
                                                                                $"?query=AU-ID({researcherScopusId})" +
                                                                                $"&start={0}" +
                                                                                $"&count={1}");

            HttpResponseMessage response = await _httpClient.SendAsync(request);
            JObject json = JObject.Parse(await response.Content.ReadAsStringAsync());

            try
            {
                return int.Parse((string)json["search-results"]["opensearch:totalResults"]);
            }
            catch
            {
                return null;
            }
        }
    }
}