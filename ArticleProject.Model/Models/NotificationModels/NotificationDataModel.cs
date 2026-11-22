namespace ArticleProject.Model.Models.NotificationModels
{
    public class NotificationDataModel
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }

        public List<NotificationTargetDataModel> Targets { get; set; }
    }
}