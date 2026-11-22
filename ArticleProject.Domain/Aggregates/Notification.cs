using ArticleProject.Domain.Common.BaseEntity;

namespace ArticleProject.Domain.Aggregates
{
    public class Notification : BaseEntity
    {
        public string Title { get; private set; }
        public string Description { get; private set; }
        public bool IsActive { get; private set; }

        public string CreatedBy { get; private set; }
        public User User { get; private set; }

        private List<NotificationTarget> _notificationTargets { get; } = new List<NotificationTarget>();
        public IReadOnlyCollection<NotificationTarget> NotificationTargets => _notificationTargets;

        public Notification()
        { }

        public Notification(string title, string description, string userId, List<NotificationTarget> targets)
        {
            Title = title;
            Description = description;
            IsActive = true;
            CreatedBy = userId;
            AddRangeNotificationTarget(targets);
        }

        public void AddRangeNotificationTarget(List<NotificationTarget> targets)
        {
            _notificationTargets.AddRange(targets);
        }

        public void AddNotificationTarget(NotificationTarget target)
        {
            _notificationTargets.Add(target);
        }

        public void Update(string title, string description, bool isActive)
        {
            Title = title;
            Description = description;
            IsActive = isActive;
        }
    }
}