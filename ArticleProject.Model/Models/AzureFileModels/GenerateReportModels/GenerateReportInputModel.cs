using ArticleProject.Model.Enums;
using Microsoft.AspNetCore.Http;

namespace ArticleProject.Model.Models.AzureFileModels.GenerateReportModels
{
    public class GenerateReportInputModel
    {
        public IFormFile File { get; set; }

        public string FileName { get; set; }
        public string Description { get; set; }
        public FileTypeEnum FileType { get; set; }
        public ReportTypeEnum ReportType { get; set; }
    }
}