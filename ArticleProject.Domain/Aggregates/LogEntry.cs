using ArticleProject.Domain.Common.Interfaces.Entity;

namespace ArticleProject.Domain.Aggregates
{
    public class LogEntry : IEntity
    {
        public long Id { get; set; }
        public string IpAddress { get; set; }
        public string RequestPath { get; set; }
        public string HttpMethod { get; set; }
        public string QueryString { get; set; }
        public string RequestBody { get; set; }
        public string StackTrace { get; set; }
        public bool IsError { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public string? UserId { get; set; }
        public User User { get; set; }

        public LogEntry()
        { }
    }
}