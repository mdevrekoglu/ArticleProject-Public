using FluentValidation;

namespace ArticleProject.Model.Models.NotificationModels.Validations
{
    public class NotificationCreateInputModelValidation : AbstractValidator<NotificationCreateInputModel>
    {
        public NotificationCreateInputModelValidation()
        {
            RuleFor(p => p.Title).NotEmpty().MaximumLength(500);
            RuleFor(p => p.Description).NotEmpty().MaximumLength(5000);
            RuleFor(p => p.UserIds).NotEmpty();
        }
    }
}