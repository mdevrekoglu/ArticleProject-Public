using FluentValidation;

namespace ArticleProject.Model.Models.UserModels.Validations
{
    public class UserPasswordResetModelValidation : AbstractValidator<UserPasswordResetModel>
    {
        public UserPasswordResetModelValidation()
        {
            RuleFor(p => p.Email).NotEmpty().MaximumLength(240).EmailAddress();
            RuleFor(p => p.NewPassword).NotEmpty().MinimumLength(6);
            RuleFor(p => p.Token).NotEmpty();
        }
    }
}