using ArticleProject.Model.Enums;

namespace ArticleProject.Model.Models.AzureFileModels
{
    public class AzureFileListDataModel
    {
        public long Id { get; set; }
        public string FileName { get; set; }
        public string FileGUID { get; set; }
        public string Description { get; set; }
        public FileTypeEnum FileType { get; set; }
        public ReportTypeEnum ReportType { get; set; }
        public DateTime CreatedDate { get; set; }

        public string UserId { get; set; }
        public string UserName { get; set; }
    }
}