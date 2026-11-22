using ArticleProject.Api.Application.Services;
using ArticleProject.Domain.Aggregates;
using ArticleProject.Domain.Common.Interfaces.Repository;
using ArticleProject.Model.Models.AffiliatonModels.JsonModels;
using ArticleProject.Model.Models.CommonModels.OutputModels;
using ArticleProject.Resources;
using MediatR;
using Microsoft.Extensions.Localization;

namespace ArticleProject.Api.Application.Commands.ScopusServiceCommands
{
    public class FetchAffiliationFromScopusCommandHandler : IRequestHandler<FetchAffiliationFromScopusCommand, MessageOutputModel>
    {
        private readonly ScopusService _scopusService;
        private readonly IRepository<Affiliation> _affiliationRepository;
        private readonly IStringLocalizer<AffiliationRes> _stringLocalizer;

        public FetchAffiliationFromScopusCommandHandler(ScopusService scopusService, IRepository<Affiliation> affiliationRepository, IStringLocalizer<AffiliationRes> stringLocalizer)
        {
            _scopusService = scopusService;
            _affiliationRepository = affiliationRepository;
            _stringLocalizer = stringLocalizer;
        }

        public async Task<MessageOutputModel> Handle(FetchAffiliationFromScopusCommand request, CancellationToken cancellationToken)
        {
            var response = new MessageOutputModel();

            var affiliationFromScopus = await _scopusService.GetAffiliationByScopusIdAsync(request.Model.Id);
            if (affiliationFromScopus == null)
            {
                response.IsSuccessed = false;
                response.Messages.Add(_stringLocalizer["ErrorNoAffiliationRecordFoundFromScopus"]);
                return response;
            }

            var affiliation = _affiliationRepository.GetByFilter(p => p.ScopusId == request.Model.Id);

            // If affiliation doesnt exist in db, add it
            if (affiliation == null)
            {
                var newEntity = new Affiliation(affiliationFromScopus.AffiliationName, affiliationFromScopus.Address, affiliationFromScopus.Country, affiliationFromScopus.InstitutionProfile.ScopusId, affiliationFromScopus.InstitutionProfile.OrgType);
                await _affiliationRepository.AddAndSaveAsync(newEntity);

                response.IsSuccessed = true;
                response.Messages.Add(_stringLocalizer["AffiliationCreatedFromScopusSuccessfully"]);
            }
            else // If affiliation exist in db, update it
            {
                if (HasChanged(affiliationFromScopus, affiliation))
                {
                    affiliation.Update(affiliationFromScopus.AffiliationName, affiliationFromScopus.Country, affiliationFromScopus.City, affiliationFromScopus.Address, affiliationFromScopus.InstitutionProfile.OrgType);
                    await _affiliationRepository.UpdateAndSaveAsync(affiliation);
                }

                response.IsSuccessed = true;
                response.Messages.Add(_stringLocalizer["AffiliationUpdatedFromScopusSuccessfully"]);
            }

            return response;
        }

        private bool HasChanged(AffiliationScopusJsonModel scopus, Affiliation db)
        {
            if (!scopus.AffiliationName.Equals(db.AffiliationName))
            {
                return true;
            }

            if (!scopus.Country.Equals(db.Country))
            {
                return true;
            }

            if (!scopus.City.Equals(db.City))
            {
                return true;
            }

            if (!scopus.Address.Equals(db.Address))
            {
                return true;
            }

            if (!scopus.Address.Equals(db.Address))
            {
                return true;
            }

            if (!scopus.InstitutionProfile.OrgType.Equals(db.OrganizationType))
            {
                return true;
            }

            return false;
        }
    }
}