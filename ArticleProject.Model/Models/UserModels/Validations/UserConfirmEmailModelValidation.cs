using FluentValidation;

namespace ArticleProject.Model.Models.UserModels.Validations
{
    public class UserConfirmEmailModelValidation : AbstractValidator<UserConfirmEmailModel>
    {
        public UserConfirmEmailModelValidation()
        {
            RuleFor(p => p.Email).NotEmpty().MaximumLength(240).EmailAddress();
            RuleFor(p => p.Token).NotEmpty();
        }
    }
}