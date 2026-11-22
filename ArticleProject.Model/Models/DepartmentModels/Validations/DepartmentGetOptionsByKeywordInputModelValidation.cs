using FluentValidation;

namespace ArticleProject.Model.Models.DepartmentModels.Validations
{
    public class DepartmentGetOptionsByKeywordInputModelValidation : AbstractValidator<DepartmentGetOptionsByKeywordInputModel>
    {
        public DepartmentGetOptionsByKeywordInputModelValidation()
        {
            RuleFor(p => p.FacultyId).NotEmpty();
            RuleFor(p => p.Keyword).NotEmpty().MinimumLength(2);
        }
    }
}