using ArticleProject.Model.Models.CommonModels.OutputModels;

namespace ArticleProject.Model.Models.AffiliatonModels
{
    public class AffiliationDataListModel : BaseListModel
    {
        public List<AffiliationDataModel> Data { get; set; }

        public AffiliationDataListModel() : base()
        {
            Data = new List<AffiliationDataModel>();
        }
    }
}