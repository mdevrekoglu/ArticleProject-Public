namespace ArticleProject.Model.Models.CommonModels.DataTransferModels
{
    public class JWTSettings
    {
        public string SecurityKey { get; set; }
        public string ValidIssuer { get; set; }
        public string ValidAudience { get; set; }
        public int ExpiryInMinutes { get; set; }
    }
}