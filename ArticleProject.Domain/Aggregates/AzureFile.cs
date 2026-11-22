using ArticleProject.Domain.Common.BaseEntity;
using ArticleProject.Model.Enums;

namespace ArticleProject.Domain.Aggregates
{
    public class AzureFile : BaseEntity
    {
        public string GUID { get; private set; }
        public string FileName { get; private set; }
        public FileTypeEnum FileType { get; private set; }
        public ReportTypeEnum ReportType { get; private set; }
        public string Description { get; private set; }

        public string CreatedBy { get; private set; }
        public User User { get; private set; }

        public AzureFile()
        { }

        public AzureFile(string guid, string fileName, FileTypeEnum fileTypeEnum, ReportTypeEnum reportTypeEnum, string description, string createdBy)
        {
            GUID = guid;
            FileName = fileName;
            FileType = fileTypeEnum;
            ReportType = reportTypeEnum;
            Description = description;
            CreatedBy = createdBy;
        }
    }
}