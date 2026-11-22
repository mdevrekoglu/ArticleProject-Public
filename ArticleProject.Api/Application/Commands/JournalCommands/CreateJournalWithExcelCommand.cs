using ArticleProject.Model.Models.CommonModels.InputModels;
using ArticleProject.Model.Models.CommonModels.OutputModels;
using MediatR;

namespace ArticleProject.Api.Application.Commands.JournalCommands
{
    public class CreateJournalWithExcelCommand : IRequest<MessageOutputModel>
    {
        public BaseFileInputModel Model { get; set; }

        public CreateJournalWithExcelCommand(BaseFileInputModel request)
        {
            Model = request;
        }
    }
}