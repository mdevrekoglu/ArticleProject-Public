using ArticleProject.Model.Enums;
using FluentValidation;

namespace ArticleProject.Model.Models.UserModels.Validations
{
    public class UserRegisterModelValidation : AbstractValidator<UserRegisterModel>
    {
        public UserRegisterModelValidation()
        {
            RuleFor(p => p.FirstName).NotEmpty().MaximumLength(30);
            RuleFor(p => p.LastName).NotEmpty().MaximumLength(20);
            RuleFor(p => p.Email).NotEmpty().MaximumLength(240);
            RuleFor(p => p.Password).NotEmpty().MinimumLength(6);
            RuleFor(p => p.UserType).NotNull().Must(x => Enum.IsDefined(typeof(UserTypeEnum), x));
        }
    }
}