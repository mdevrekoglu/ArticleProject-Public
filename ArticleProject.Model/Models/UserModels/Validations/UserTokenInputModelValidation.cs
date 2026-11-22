using FluentValidation;

namespace ArticleProject.Model.Models.UserModels.Validations
{
    public class UserTokenInputModelValidation : AbstractValidator<UserTokenInputModel>
    {
        public UserTokenInputModelValidation()
        {
            RuleFor(p => p.Token).NotEmpty();
            RuleFor(p => p.RefreshToken).NotEmpty().MaximumLength(128);
        }
    }
}