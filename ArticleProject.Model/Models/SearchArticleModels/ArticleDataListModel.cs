using ArticleProject.Model.Models.CommonModels.OutputModels;

namespace ArticleProject.Model.Models.SearchArticleModels
{
    public class ArticleDataListModel : BaseListModel
    {
        public List<ArticleDataModel> Data { get; set; }

        public ArticleDataListModel() : base()
        {
            Data = new List<ArticleDataModel>();
        }
    }
}