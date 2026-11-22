namespace ArticleProject.Model.Models.NotificationModels
{
    public class NotificationUserDataModel
    {
        public bool IsRead { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}