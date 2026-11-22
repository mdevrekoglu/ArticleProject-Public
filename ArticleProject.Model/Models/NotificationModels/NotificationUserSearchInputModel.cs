using ArticleProject.Model.Models.CommonModels.InputModels;

namespace ArticleProject.Model.Models.NotificationModels
{
    public class NotificationUserSearchInputModel : BaseSearchModel
    {
        public NotificationUserSearchInputModel() : base()
        {
            SortingColumn = "createdDate";
            SortingType = "desc";
        }
    }
}