using ArticleProject.Domain.Common.BaseEntity;

namespace ArticleProject.Domain.Aggregates
{
    public class NotificationTarget : BaseEntity
    {
        public bool IsRead { get; set; }

        public string UserId { get; private set; }
        public User User { get; private set; }

        public long NotificationId { get; private set; }
        public Notification Notification { get; private set; }

        public NotificationTarget()
        { }

        public NotificationTarget(string userId)
        {
            UserId = userId;
        }

        public NotificationTarget(string userId, long notificationId)
        {
            IsRead = false;
            UserId = userId;
            NotificationId = notificationId;
        }
    }
}