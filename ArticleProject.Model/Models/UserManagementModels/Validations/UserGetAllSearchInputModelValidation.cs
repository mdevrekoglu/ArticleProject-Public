using FluentValidation;

namespace ArticleProject.Model.Models.UserManagementModels.Validations
{
    public class UserGetAllSearchInputModelValidation : AbstractValidator<UserOptionsSearchInputModel>
    {
        public UserGetAllSearchInputModelValidation()
        {
            RuleFor(p => p).Must(p => p.AffiliationId.HasValue || p.FacultyId.HasValue || p.DepartmentId.HasValue);
        }
    }
}