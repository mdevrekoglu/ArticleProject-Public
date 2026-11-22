using FluentValidation;

namespace ArticleProject.Model.Models.FacultyModels.Validations
{
    public class FacultyUpdateInputModelValidation : AbstractValidator<FacultyUpdateInputModel>
    {
        public FacultyUpdateInputModelValidation()
        {
            RuleFor(p => p.Id).NotEmpty();
            RuleFor(p => p.FacultyName).NotEmpty().MaximumLength(100);
            RuleFor(p => p.AffiliationId).NotEmpty();
        }
    }
}