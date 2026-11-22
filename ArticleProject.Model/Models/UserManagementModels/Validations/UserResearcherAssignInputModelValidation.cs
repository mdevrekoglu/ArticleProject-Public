using FluentValidation;

namespace ArticleProject.Model.Models.UserManagementModels.Validations
{
    public class UserResearcherAssignInputModelValidation : AbstractValidator<UserResearcherAssignInputModel>
    {
        public UserResearcherAssignInputModelValidation()
        {
            RuleFor(p => p.UserId).NotEmpty();
            RuleFor(p => p.ResearcherId).NotEmpty();
        }
    }
}