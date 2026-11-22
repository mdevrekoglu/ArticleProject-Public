namespace ArticleProject.Model.Models.NotificationModels
{
    public class NotificationUpdateInputModel
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public List<string> UserIds { get; set; }
    }
}