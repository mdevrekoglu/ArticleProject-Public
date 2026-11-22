using ArticleProject.Domain.Aggregates;

namespace ArticleProject.Domain.Common.Interfaces.Repository
{
    public interface ILogRepository
    {
        Task AddLogAsync(LogEntry logEntry);
    }
}