using ArticleProject.Model.Models.AffiliatonModels;
using ArticleProject.Model.Models.CommonModels.OutputModels;
using MediatR;

namespace ArticleProject.Api.Application.Commands.AffiliationCommands
{
    public class UpdateAffiliationCommand : IRequest<KeyOutputModel>
    {
        public AffiliationUpdateInputModel Model { get; set; }

        public UpdateAffiliationCommand(AffiliationUpdateInputModel request)
        {
            Model = request;
        }
    }
}