using FluentValidation;

namespace ArticleProject.Model.Models.MajorDisciplineModels.Validations
{
    public class MajorDisciplineUpdateInputModelValidation : AbstractValidator<MajorDisciplineUpdateInputModel>
    {
        public MajorDisciplineUpdateInputModelValidation()
        {
            RuleFor(p => p.Id).NotEmpty();
            RuleFor(p => p.MajorDisciplineName).NotEmpty().MaximumLength(100);
            RuleFor(p => p.DepartmentId).NotEmpty();
        }
    }
}