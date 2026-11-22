using ArticleProject.Model.Enums;
using FluentValidation;

namespace ArticleProject.Model.Models.UserModels.Validations
{
    public class UserLogInModelValidation : AbstractValidator<UserLogInModel>
    {
        public UserLogInModelValidation()
        {
            RuleFor(p => p.Email).NotEmpty().MaximumLength(240);
            RuleFor(p => p.Password).NotEmpty().MinimumLength(6);
            RuleFor(p => p.UserType).NotNull().Must(x => Enum.IsDefined(typeof(UserTypeEnum), x));
            RuleFor(p => p.RememberMe).NotNull();
        }
    }
}