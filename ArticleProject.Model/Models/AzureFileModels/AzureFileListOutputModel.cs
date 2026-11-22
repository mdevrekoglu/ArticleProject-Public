using ArticleProject.Model.Models.CommonModels.OutputModels;

namespace ArticleProject.Model.Models.AzureFileModels
{
    public class AzureFileListOutputModel : BaseListModel
    {
        public List<AzureFileListDataModel> Data { get; set; }

        public AzureFileListOutputModel() : base()
        {
            Data = new List<AzureFileListDataModel>();
        }
    }
}