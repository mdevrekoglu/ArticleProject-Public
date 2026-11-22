using ArticleProject.Model.Models.CommonModels.OutputModels;

namespace ArticleProject.Model.Models.AzureFileModels
{
    public class AzureFileDownloadModel : MessageOutputModel
    {
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public byte[] FileContent { get; set; }
    }
}