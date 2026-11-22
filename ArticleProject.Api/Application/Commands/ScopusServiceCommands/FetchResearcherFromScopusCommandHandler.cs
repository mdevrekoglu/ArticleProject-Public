using ArticleProject.Api.Application.Services;
using ArticleProject.Domain.Aggregates;
using ArticleProject.Domain.Common.Exceptions;
using ArticleProject.Domain.Common.Interfaces.Repository;
using ArticleProject.Model.Models.CommonModels.OutputModels;
using ArticleProject.Model.Models.ResearcherModels.JsonModels;
using ArticleProject.Resources;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace ArticleProject.Api.Application.Commands.ScopusServiceCommands
{
    public class FetchResearcherFromScopusCommandHandler : IRequestHandler<FetchResearcherFromScopusCommand, MessageOutputModel>
    {
        private readonly IRepository<Researcher> _researcherRepository;
        private readonly IReadOnlyRepository<Affiliation> _affiliationRepository;
        private readonly ScopusService _scopusService;
        private readonly IStringLocalizer<ResearcherRes> _stringLocalizer;

        public FetchResearcherFromScopusCommandHandler(IRepository<Researcher> researcherRepository, IReadOnlyRepository<Affiliation> affiliationRepository, ScopusService scopusService, IStringLocalizer<ResearcherRes> stringLocalizer)
        {
            _researcherRepository = researcherRepository;
            _affiliationRepository = affiliationRepository;
            _scopusService = scopusService;
            _stringLocalizer = stringLocalizer;
        }

        public async Task<MessageOutputModel> Handle(FetchResearcherFromScopusCommand request, CancellationToken cancellationToken)
        {
            var response = new MessageOutputModel();

            var researcherFromScopus = (await _scopusService.GetResearcherByScopusIdAsync(request.Model.Id));
            if (researcherFromScopus == null)
            {
                response.IsSuccessed = false;
                response.Messages.Add(_stringLocalizer["ErrorNoResearcherRecordFoundFromScopus"]);
                return response;
            }

            var researcher = _researcherRepository.GetByFilter(p => p.ScopusId == request.Model.Id, i => i.Include(p => p.Affiliation));

            // If user doesnt exist in db, add it
            if (researcher == null)
            {
                var affiliation = _affiliationRepository.GetByFilter(p => p.ScopusId == researcherFromScopus.AffiliationCurrent.AffiliationId).ThrowIfNull(_stringLocalizer["ErrorNoAffiliationRecordFoundByScopusId"]);

                if (affiliation == null)
                {
                    response.IsSuccessed = false;
                    response.Messages.Add(_stringLocalizer["ErrorNoAffiliationRecordFoundByScopusId"]);
                    return response;
                }

                var newEntity = new Researcher
                {
                    FirstName = researcherFromScopus.PreferredName.GivenName,
                    LastName = researcherFromScopus.PreferredName.Surname,
                    ScopusId = long.Parse(researcherFromScopus.Identifier.Split(':')[1].Trim()), // Remove first part AUTHOR_ID:
                    AffiliationId = affiliation.Id,
                    DocumentCount = researcherFromScopus.DocumentCount,
                    ORCId = (!string.IsNullOrEmpty(researcherFromScopus.Orcid) && researcherFromScopus.Orcid.Length > 2) ? researcherFromScopus.Orcid.Substring(1, researcherFromScopus.Orcid.Length - 2) : null, // Remove []
                    WoSId = null,
                    IsActive = true,
                };

                await _researcherRepository.AddAndSaveAsync(newEntity);

                response.IsSuccessed = true;
                response.Messages.Add(_stringLocalizer["ResearcherCreatedFromScopusSuccessfully"]);
            }
            else // If user exists in db, update it
            {
                if (HasChanged(researcherFromScopus, researcher))
                {
                    researcher.Update(researcherFromScopus.AffiliationCurrent.AffiliationId, researcherFromScopus.DocumentCount);
                    await _researcherRepository.UpdateAndSaveAsync(researcher);
                }

                response.IsSuccessed = true;
                response.Messages.Add(_stringLocalizer["ResearcherUpdatedFromScopusSuccessfully"]);
            }

            return response;
        }

        private bool HasChanged(ResearcherScopusJsonModel scopus, Researcher db)
        {
            if (scopus.DocumentCount != db.DocumentCount)
            {
                return true;
            }

            if (scopus.AffiliationCurrent.AffiliationId != db.Affiliation.ScopusId)
            {
                return true;
            }

            return false;
        }
    }
}