using ArticleProject.Model.Models.AffiliatonModels;
using MediatR;

namespace ArticleProject.Api.Application.Commands.AffiliationCommands
{
    public class CreateAffiliationCommand : IRequest<AffiliationDataOutputModel>
    {
        public AffiliationCreateInputModel Model { get; set; }

        public CreateAffiliationCommand(AffiliationCreateInputModel request)
        {
            Model = request;
        }
    }
}