using ArticleProject.Model.Models.AzureFileModels.GenerateReportModels;
using ArticleProject.Model.Models.CommonModels.OutputModels;
using MediatR;

namespace ArticleProject.Api.Application.Commands.AzureFileCommands
{
    public class GenerateReportCommand : IRequest<MessageOutputModel>
    {
        public GenerateReportInputModel Model { get; set; }

        public GenerateReportCommand(GenerateReportInputModel request)
        {
            Model = request;
        }
    }
}