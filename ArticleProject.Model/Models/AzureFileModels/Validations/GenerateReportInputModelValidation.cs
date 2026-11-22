using ArticleProject.Model.Models.AzureFileModels.GenerateReportModels;
using FluentValidation;

namespace ArticleProject.Model.Models.AzureFileModels.Validations
{
    public class GenerateReportInputModelValidation : AbstractValidator<GenerateReportInputModel>
    {
        public GenerateReportInputModelValidation()
        {
            RuleFor(p => p.File).NotEmpty();
            RuleFor(p => p.FileName).NotEmpty().MaximumLength(255);
            RuleFor(p => p.Description).NotEmpty().MaximumLength(500);
            RuleFor(p => p.FileType).NotNull().IsInEnum();
            RuleFor(p => p.ReportType).NotNull().IsInEnum();
        }
    }
}