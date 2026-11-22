namespace ArticleProject.Model.Models.NotificationModels
{
    public class NotificationCreateInputModel
    {
        public string Title { get; set; }
        public string Description { get; set; }

        public List<string> UserIds { get; set; }
    }
}