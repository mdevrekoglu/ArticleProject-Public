using ArticleProject.Model.Models.CommonModels.OutputModels;

namespace ArticleProject.Model.Models.NotificationModels
{
    public class NotificationUserDataListModel : BaseListModel
    {
        public List<NotificationUserDataModel> Data { get; set; }

        public NotificationUserDataListModel() : base()
        {
            Data = new List<NotificationUserDataModel>();
        }
    }
}