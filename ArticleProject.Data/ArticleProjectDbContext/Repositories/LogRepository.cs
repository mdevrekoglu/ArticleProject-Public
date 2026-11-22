using ArticleProject.Domain.Aggregates;
using ArticleProject.Domain.Common.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;

namespace ArticleProject.Data.ArticleProjectDbContext.Repositories
{
    public class LogRepository : ILogRepository
    {
        private readonly ArticleProjectContext _context;
        private readonly DbSet<LogEntry> _dbSet;

        public LogRepository(ArticleProjectContext context)
        {
            _context = context;
            _dbSet = context.Set<LogEntry>();
        }

        public async Task AddLogAsync(LogEntry logEntry)
        {
            await _dbSet.AddAsync(logEntry);
            await _context.SaveChangesAsync();
        }
    }
}