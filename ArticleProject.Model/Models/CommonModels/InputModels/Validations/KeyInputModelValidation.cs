using FluentValidation;

namespace ArticleProject.Model.Models.CommonModels.InputModels.Validations
{
    public class KeyInputModelValidation : AbstractValidator<KeyInputModel>
    {
        public KeyInputModelValidation()
        {
            RuleFor(p => p.Id).NotEmpty();
        }
    }
}