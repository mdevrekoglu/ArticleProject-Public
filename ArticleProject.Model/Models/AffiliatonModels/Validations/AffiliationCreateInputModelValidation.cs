using FluentValidation;

namespace ArticleProject.Model.Models.AffiliatonModels.Validations
{
    public class AffiliationCreateInputModelValidation : AbstractValidator<AffiliationCreateInputModel>
    {
        public AffiliationCreateInputModelValidation()
        {
            RuleFor(p => p.AffiliationName).NotEmpty().MaximumLength(100);
            RuleFor(p => p.Address).NotEmpty().MaximumLength(200);
            RuleFor(p => p.City).NotEmpty().MaximumLength(50);
            RuleFor(p => p.Country).NotEmpty().MaximumLength(50);
            RuleFor(p => p.ScopusId).NotEmpty();
            RuleFor(p => p.OrganizationType).NotEmpty().MaximumLength(50);
        }
    }
}