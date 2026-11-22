using ArticleProject.Model.Enums;
using FluentValidation;

namespace ArticleProject.Model.Models.UserModels.Validations
{
    public class UserRequestValidation : AbstractValidator<UserRequestModel>
    {
        public UserRequestValidation()
        {
            RuleFor(p => p.Email).NotEmpty().MaximumLength(240);
            RuleFor(p => p.UserType).NotNull().Must(x => Enum.IsDefined(typeof(UserTypeEnum), x));
        }
    }
}