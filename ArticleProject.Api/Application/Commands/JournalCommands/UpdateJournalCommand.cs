using ArticleProject.Model.Models.CommonModels.OutputModels;
using ArticleProject.Model.Models.JournalModels;
using MediatR;

namespace ArticleProject.Api.Application.Commands.JournalCommands
{
    public class UpdateJournalCommand : IRequest<KeyOutputModel>
    {
        public JournalUpdateInputModel Model { get; set; }

        public UpdateJournalCommand(JournalUpdateInputModel request)
        {
            Model = request;
        }
    }
}