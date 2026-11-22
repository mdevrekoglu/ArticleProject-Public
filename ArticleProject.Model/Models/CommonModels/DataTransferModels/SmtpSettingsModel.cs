namespace ArticleProject.Model.Models.CommonModels.DataTransferModels
{
    public class SmtpSettingsModel
    {
        public string Server { get; set; }
        public int Port { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool EnableSsl { get; set; }
    }
}