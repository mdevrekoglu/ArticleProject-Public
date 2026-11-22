using FluentValidation;

namespace ArticleProject.Model.Models.FacultyModels.Validations
{
    public class FacultyCreateInputModelValidation : AbstractValidator<FacultyCreateInputModel>
    {
        public FacultyCreateInputModelValidation()
        {
            RuleFor(p => p.FacultyName).NotEmpty().MaximumLength(100);
            RuleFor(p => p.AffiliationId).NotEmpty();
        }
    }
}