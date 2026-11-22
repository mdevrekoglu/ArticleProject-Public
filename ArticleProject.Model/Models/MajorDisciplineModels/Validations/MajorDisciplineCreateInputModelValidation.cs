using FluentValidation;

namespace ArticleProject.Model.Models.MajorDisciplineModels.Validations
{
    public class MajorDisciplineCreateInputModelValidation : AbstractValidator<MajorDisciplineCreateInputModel>
    {
        public MajorDisciplineCreateInputModelValidation()
        {
            RuleFor(p => p.MajorDisciplineName).NotEmpty().MaximumLength(100);
            RuleFor(p => p.DepartmentId).NotEmpty();
        }
    }
}