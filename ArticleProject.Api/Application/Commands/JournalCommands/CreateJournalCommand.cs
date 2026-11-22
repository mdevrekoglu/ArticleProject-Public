using ArticleProject.Model.Models.JournalModels;
using MediatR;

namespace ArticleProject.Api.Application.Commands.JournalCommands
{
    public class CreateJournalCommand : IRequest<JournalDataOutputModel>
    {
        public JournalCreateInputModel Model { get; set; }

        public CreateJournalCommand(JournalCreateInputModel request)
        {
            Model = request;
        }
    }
}