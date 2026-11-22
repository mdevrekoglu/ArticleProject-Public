using FluentValidation;

namespace ArticleProject.Model.Models.SearchArticleModels.Validations
{
    public class ArticleSearchInputModelValidation : AbstractValidator<ArticleSearchInputModel>
    {
        public ArticleSearchInputModelValidation()
        {
            // Check if at least one of the properties is not null or empty
            RuleFor(x => x)
                .Must(x => !string.IsNullOrEmpty(x.Title) || x.AffiliationId != null || x.PublishYear != null || !string.IsNullOrEmpty(x.DOI)
                        || (x.ResearcherIds != null && x.ResearcherIds.Any()) || (x.JournalIds != null && x.JournalIds.Any()));
        }
    }
}