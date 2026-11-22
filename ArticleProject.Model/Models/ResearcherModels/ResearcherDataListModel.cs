using ArticleProject.Model.Models.CommonModels.OutputModels;

namespace ArticleProject.Model.Models.ResearcherModels
{
    public class ResearcherDataListModel : BaseListModel
    {
        public List<ResearcherDataModel> Data { get; set; }

        public ResearcherDataListModel() : base()
        {
            Data = new List<ResearcherDataModel>();
        }
    }
}