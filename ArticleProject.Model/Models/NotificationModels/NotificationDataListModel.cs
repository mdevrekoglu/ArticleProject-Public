using ArticleProject.Model.Models.CommonModels.OutputModels;

namespace ArticleProject.Model.Models.NotificationModels
{
    public class NotificationDataListModel : BaseListModel
    {
        public List<NotificationDataModel> Data { get; set; }
    }
}