using ArticleProject.Domain.Common.Exceptions;
using ArticleProject.Domain.Common.Interfaces.Entity;
using ArticleProject.Domain.Common.Interfaces.Rule;

namespace ArticleProject.Domain.Common.BaseEntity
{
    public abstract class BaseEntity : IEntity
    {
        public long Id { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        protected static void CheckRule(IBusinessRule rule)
        {
            if (rule.IsBroken())
                throw new BusinessRuleValidationException(rule);
        }
    }
}