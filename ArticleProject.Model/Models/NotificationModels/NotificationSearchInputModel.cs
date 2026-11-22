using ArticleProject.Model.Models.CommonModels.InputModels;

namespace ArticleProject.Model.Models.NotificationModels
{
    public class NotificationSearchInputModel : BaseSearchModel
    {
        public string? Title { get; set; }
        public bool? IsActive { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public NotificationSearchInputModel() : base()
        {
            SortingColumn = "createdDate";
            SortingType = "asc";
        }
    }
}