namespace ArticleProject.Domain.Common.Interfaces.Rule
{
    public interface IBusinessRule
    {
        bool IsBroken();

        string Message { get; }
    }
}