using ArticleProject.Domain.Aggregates;
using ArticleProject.Domain.Common.Interfaces.Repository;
using ArticleProject.Model.Models.AffiliatonModels;
using ArticleProject.Resources;
using MediatR;
using Microsoft.Extensions.Localization;

namespace ArticleProject.Api.Application.Commands.AffiliationCommands
{
    public class CreateAffiliationCommandHandler : IRequestHandler<CreateAffiliationCommand, AffiliationDataOutputModel>
    {
        private readonly IRepository<Affiliation> _affiliationRepository;
        private readonly IStringLocalizer<AffiliationRes> _stringLocalizer;

        public CreateAffiliationCommandHandler(IRepository<Affiliation> affiliationRepository, IStringLocalizer<AffiliationRes> stringLocalizer)
        {
            _affiliationRepository = affiliationRepository;
            _stringLocalizer = stringLocalizer;
        }

        public async Task<AffiliationDataOutputModel> Handle(CreateAffiliationCommand request, CancellationToken cancellationToken)
        {
            var response = new AffiliationDataOutputModel();

            // Check uniqueness rule
            var existingEntityCount = _affiliationRepository.GetCountByFilter(p => p.AffiliationName == request.Model.AffiliationName || p.ScopusId == request.Model.ScopusId);

            if (existingEntityCount > 0)
            {
                response.IsSuccessed = false;
                response.Messages.Add(_stringLocalizer["ErrorAffiliationMustBeUniqueToCreate"]);
                return response;
            }

            var entity = new Affiliation
            {
                AffiliationName = request.Model.AffiliationName,
                Address = request.Model.Address,
                City = request.Model.City,
                Country = request.Model.Country,
                ScopusId = request.Model.ScopusId,
                OrganizationType = request.Model.OrganizationType
            };

            await _affiliationRepository.AddAndSaveAsync(entity);

            response = new AffiliationDataOutputModel
            {
                Id = entity.Id,
                AffiliationName = entity.AffiliationName,
                Address = entity.Address,
                City = entity.City,
                Country = entity.Country,
                ScopusId = entity.ScopusId,
                OrganizationType = entity.OrganizationType
            };

            response.IsSuccessed = true;
            response.Messages.Add(_stringLocalizer["AffiliationCreatedSuccessfully"]);

            return response;
        }
    }
}