using ArticleProject.Domain.Common.Interfaces.Rule;

namespace ArticleProject.Domain.Common.Exceptions
{
    public class BusinessRuleValidationException : Exception
    {
        public IBusinessRule BrokenRule { get; }

        public BusinessRuleValidationException(IBusinessRule rule) : base(rule.Message)
        {
            BrokenRule = rule;
        }
    }
}