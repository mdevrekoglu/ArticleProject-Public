using FluentValidation;

namespace ArticleProject.Model.Models.FacultyModels.Validations
{
    public class FacultyGetOptionsByKeywordInputModelValidation : AbstractValidator<FacultyGetOptionsByKeywordInputModel>
    {
        public FacultyGetOptionsByKeywordInputModelValidation()
        {
            RuleFor(p => p.Keyword).NotEmpty().MinimumLength(2);
            RuleFor(p => p.AffiliationId).NotEmpty();
        }
    }
}