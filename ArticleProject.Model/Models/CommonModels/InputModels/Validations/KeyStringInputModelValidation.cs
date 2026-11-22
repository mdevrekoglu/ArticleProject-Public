using FluentValidation;

namespace ArticleProject.Model.Models.CommonModels.InputModels.Validations
{
    public class KeyStringInputModelValidation : AbstractValidator<KeyStringInputModel>
    {
        public KeyStringInputModelValidation()
        {
            RuleFor(p => p.Id).NotEmpty();
        }
    }
}