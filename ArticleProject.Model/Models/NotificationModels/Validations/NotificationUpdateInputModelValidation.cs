using FluentValidation;

namespace ArticleProject.Model.Models.NotificationModels.Validations
{
    public class NotificationUpdateInputModelValidation : AbstractValidator<NotificationUpdateInputModel>
    {
        public NotificationUpdateInputModelValidation()
        {
            RuleFor(p => p.Id).NotEmpty();
            RuleFor(p => p.Title).NotEmpty().MaximumLength(500);
            RuleFor(p => p.Description).NotEmpty().MaximumLength(5000);
            RuleFor(p => p.IsActive).NotNull();
            RuleFor(p => p.UserIds).NotEmpty();
        }
    }
}