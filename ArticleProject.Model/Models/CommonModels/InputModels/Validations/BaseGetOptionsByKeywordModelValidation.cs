using FluentValidation;

namespace ArticleProject.Model.Models.CommonModels.InputModels.Validations
{
    public class BaseGetOptionsByKeywordModelValidation : AbstractValidator<BaseGetOptionsByKeywordModel>
    {
        public BaseGetOptionsByKeywordModelValidation()
        {
            RuleFor(p => p.Keyword).NotEmpty().MinimumLength(2);
            RuleFor(p => p.OptionsSize).GreaterThan(0);
        }
    }
}