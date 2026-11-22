using FluentValidation;

namespace ArticleProject.Model.Models.UserModels.Validations
{
    public class UserChangePasswordModelValidation : AbstractValidator<UserChangePasswordModel>
    {
        public UserChangePasswordModelValidation()
        {
            RuleFor(p => p.Email).NotEmpty().MaximumLength(240).EmailAddress();
            RuleFor(p => p.Password).NotEmpty().MinimumLength(6);
            RuleFor(p => p.NewPassword).NotEmpty().MinimumLength(6);
        }
    }
}