using FluentValidation;

namespace ArticleProject.Model.Models.JournalModels.Validations
{
    public class JournalUpdateInputModelValidation : AbstractValidator<JournalUpdateInputModel>
    {
        public JournalUpdateInputModelValidation()
        {
            RuleFor(p => p.Id).NotEmpty();
            RuleFor(p => p.Name).NotEmpty().MaximumLength(300);
            RuleFor(p => p.AbbreviationName).NotEmpty().MaximumLength(200);
            RuleFor(p => p.ISSN).NotEmpty().MaximumLength(9);
            RuleFor(p => p.EISSN).NotEmpty().MaximumLength(9);
            RuleFor(p => p.JIF).NotEmpty();
            RuleFor(p => p.JIF5Years).NotEmpty();
            //RuleFor(p => p.CategoryId).NotEmpty();
            RuleFor(p => p.CategoryName).NotEmpty().MaximumLength(300);
            RuleFor(p => p.Quartile).NotEmpty();
            RuleFor(p => p.Rank).NotEmpty();
            RuleFor(p => p.TotalJournals).NotEmpty();
            RuleFor(p => p.Year).NotEmpty();
        }
    }
}