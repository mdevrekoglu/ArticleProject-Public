namespace ArticleProject.Model.Models.CommonModels.DataTransferModels
{
    public class AzureBlobSettingsModel
    {
        public string AccountName { get; set; }
        public string AccountKey { get; set; }
        public string ContainerName { get; set; }
        public string BlobUrl { get; set; }
    }
}