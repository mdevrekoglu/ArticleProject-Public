using ArticleProject.Domain.Common.Interfaces.Service;
using ArticleProject.Model.Models.CommonModels.DataTransferModels;
using ArticleProject.Model.Models.ResearcherModels.JsonModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ArticleProject.Api.Application.Services
{
    public class ORCService : IBaseSingletonService
    {
        private readonly HttpClient _httpClient;
        private readonly ORCSettingsModel _ORCSettings;

        public ORCService(IConfiguration configuration)
        {
            _ORCSettings = configuration.GetSection("ORCSettings").Get<ORCSettingsModel>()!;

            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(_ORCSettings.BaseLink)
            };

            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        }

        public async Task<List<ORCExternalIdentifierJsonModel>> GetExternalIdentifiers(string ORCId)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, $"{ORCId}{_ORCSettings.ExternalIdentifierLink}");

            HttpResponseMessage response = await _httpClient.SendAsync(request);
            JObject json = JObject.Parse(await response.Content.ReadAsStringAsync());

            try
            {
                return JsonConvert.DeserializeObject<List<ORCExternalIdentifierJsonModel>>(json["external-identifier"].ToString());
            }
            catch
            {
                return null;
            }
        }
    }
}