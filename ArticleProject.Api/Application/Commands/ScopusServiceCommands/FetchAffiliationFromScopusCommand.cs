using ArticleProject.Model.Models.CommonModels.InputModels;
using ArticleProject.Model.Models.CommonModels.OutputModels;
using MediatR;

namespace ArticleProject.Api.Application.Commands.ScopusServiceCommands
{
    public class FetchAffiliationFromScopusCommand : IRequest<MessageOutputModel>
    {
        public KeyInputModel Model { get; set; }

        public FetchAffiliationFromScopusCommand(KeyInputModel request)
        {
            Model = request;
        }
    }
}