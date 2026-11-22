using ArticleProject.Model.Models.CommonModels.OutputModels;

namespace ArticleProject.Model.Models.JournalModels
{
    public class JournalDataListModel : BaseListModel
    {
        public List<JournalDataModel> Data { get; set; }

        public JournalDataListModel() : base()
        {
            Data = new List<JournalDataModel>();
        }
    }
}