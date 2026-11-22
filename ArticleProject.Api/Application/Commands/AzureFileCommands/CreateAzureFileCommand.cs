using ArticleProject.Model.Models.CommonModels.InputModels;
using ArticleProject.Model.Models.CommonModels.OutputModels;
using MediatR;

namespace ArticleProject.Api.Application.Commands.AzureFileCommands
{
    public class CreateAzureFileCommand : IRequest<MessageOutputModel>
    {
        public BaseFileInputModel Model { get; set; }

        public CreateAzureFileCommand(BaseFileInputModel request)
        {
            Model = request;
        }
    }
}