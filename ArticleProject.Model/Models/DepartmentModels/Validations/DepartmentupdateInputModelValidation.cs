using FluentValidation;

namespace ArticleProject.Model.Models.DepartmentModels.Validations
{
    public class DepartmentUpdateInputModelValidation : AbstractValidator<DepartmentUpdateInputModel>
    {
        public DepartmentUpdateInputModelValidation()
        {
            RuleFor(p => p.Id).NotEmpty();
            RuleFor(p => p.DepartmentName).NotEmpty().MaximumLength(100);
            RuleFor(p => p.StudentCount).NotEmpty();
            RuleFor(p => p.ResearcherCount).NotEmpty();
            RuleFor(p => p.FacultyId).NotEmpty();
        }
    }
}