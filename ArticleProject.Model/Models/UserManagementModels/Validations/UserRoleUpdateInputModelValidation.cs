using FluentValidation;

namespace ArticleProject.Model.Models.UserManagementModels.Validations
{
    public class UserRoleUpdateInputModelValidation : AbstractValidator<UserRoleUpdateInputModel>
    {
        public UserRoleUpdateInputModelValidation()
        {
            RuleFor(p => p.UserId).NotEmpty();
            RuleFor(p => p.RoleIds).NotEmpty();
        }
    }
}