using FluentValidation;

namespace ArticleProject.Model.Models.UserModels.Validations
{
    public class UserImageChangeModelValidation : AbstractValidator<UserImageChangeModel>
    {
        public UserImageChangeModelValidation()
        {
            RuleFor(p => p.Image).NotEmpty().MaximumLength(1000000);
        }
    }
}