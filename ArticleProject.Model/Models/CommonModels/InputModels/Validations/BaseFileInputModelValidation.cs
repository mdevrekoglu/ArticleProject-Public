using FluentValidation;

namespace ArticleProject.Model.Models.CommonModels.InputModels.Validations
{
    public class BaseFileInputModelValidation : AbstractValidator<BaseFileInputModel>
    {
        public BaseFileInputModelValidation()
        {
            RuleFor(p => p.File).NotEmpty();
        }
    }
}