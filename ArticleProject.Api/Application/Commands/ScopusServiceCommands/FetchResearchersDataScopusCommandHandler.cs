using ArticleProject.Api.Application.Services;
using ArticleProject.Domain.Aggregates;
using ArticleProject.Domain.Common.Interfaces.Repository;
using ArticleProject.Model.Constants;
using ArticleProject.Model.Models.ResearcherModels.JsonModels;
using MediatR;

namespace ArticleProject.Api.Application.Commands.ScopusServiceCommands
{
    public class FetchResearchersDataScopusCommandHandler : IRequestHandler<FetchResearchersDataScopusCommand, bool>
    {
        private readonly ScopusService _scopusService;
        private readonly IRepository<Researcher> _researcherRepository;
        private readonly IReadOnlyRepository<Affiliation> _affiliationRepository;
        private readonly ORCService _orcService;
        private readonly int _count = 200;

        public FetchResearchersDataScopusCommandHandler(ScopusService scopusService, IRepository<Researcher> researcherRepository, IReadOnlyRepository<Affiliation> affiliationRepository, ORCService orcService)
        {
            _scopusService = scopusService;
            _researcherRepository = researcherRepository;
            _affiliationRepository = affiliationRepository;
            _orcService = orcService;
        }

        public async Task<bool> Handle(FetchResearchersDataScopusCommand request, CancellationToken cancellationToken)
        {
            var affiliations = _affiliationRepository.GetAllByFilter().Select(p => new { p.Id, p.ScopusId }).ToList();

            foreach (var affiliation in affiliations)
            {
                var totalResearcher = await _scopusService.GetResearcherCountAsync(affiliation.ScopusId);

                if (!totalResearcher.HasValue || totalResearcher.Value == 0)
                {
                    continue;
                }

                var dbResearchers = _researcherRepository.GetAllByFilter(p => p.AffiliationId == affiliation.Id).ToList();
                var dbResearcherIds = dbResearchers.Select(p => p.ScopusId).ToList();

                int callCount = (int)Math.Ceiling((double)totalResearcher.Value / _count);
                callCount = callCount > 25 ? 25 : callCount; // Scopus 5000 üzerine çıkarttırmıyor ??
                List<ResearcherScopusJsonModel> allResearchers = new List<ResearcherScopusJsonModel>();
                for (int i = 0; i < callCount; i++)
                {
                    allResearchers.AddRange((await _scopusService.GetResearchersAsync(affiliation.ScopusId, i * _count, _count)));
                }

                // Scopus might send some duplicate researchers for that reason we need to distinct them
                var scopusSet = new HashSet<long>();
                var eidSet = new HashSet<string>();
                var orcidSet = new HashSet<string>();
                allResearchers = allResearchers
                    .Where(r =>
                    {
                        bool isDuplicate =
                            !scopusSet.Add(r.ScopusId) ||
                            (!string.IsNullOrEmpty(r.Eid) && !eidSet.Add(r.Eid)) ||
                            (!string.IsNullOrEmpty(r.Orcid) && !orcidSet.Add(r.Orcid));

                        return !isDuplicate;
                    })
                    .ToList();

                var newResearchers = new List<Researcher>();
                foreach (var researcher in allResearchers)
                {
                    var dbResearcher = dbResearchers.FirstOrDefault(p => p.ScopusId == researcher.ScopusId);
                    if (dbResearcher != null && HasChanged(researcher, dbResearcher))
                    {
                        dbResearcher.Update(researcher.AffiliationCurrent.AffiliationId, researcher.DocumentCount);
                    }
                    else
                    {
                        string wosId = null;
                        if (!string.IsNullOrEmpty(researcher.Orcid))
                        {
                            var externalIdentifiers = await _orcService.GetExternalIdentifiers(researcher.Orcid);

                            if (externalIdentifiers != null && externalIdentifiers.Any())
                                wosId = externalIdentifiers.FirstOrDefault(p => p.ExternalIdType.Equals(ResearcherExternalTypes.WoSTypeName))?.ExternalIdValue;
                        }

                        newResearchers.Add(new Researcher(researcher.PreferredName.GivenName, researcher.PreferredName.Surname,
                                                    null, researcher.DocumentCount, null, null, affiliation.Id, null,
                                                    wosId, researcher.Orcid, researcher.ScopusId));
                    }
                }
                await _researcherRepository.AddRangeAndSaveAsync(newResearchers);
            }
            return true;
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