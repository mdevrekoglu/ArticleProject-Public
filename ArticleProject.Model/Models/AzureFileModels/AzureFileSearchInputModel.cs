using ArticleProject.Model.Enums;
using ArticleProject.Model.Models.CommonModels.InputModels;

namespace ArticleProject.Model.Models.AzureFileModels
{
    public class AzureFileSearchInputModel : BaseSearchModel
    {
        public string? FileName { get; set; }
        public string? FileGUID { get; set; }
        public FileTypeEnum? FileType { get; set; }
        public ReportTypeEnum? ReportType { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public AzureFileSearchInputModel() : base()
        {
            SortingColumn = "fileName";
            SortingType = "asc";
        }
    }
}