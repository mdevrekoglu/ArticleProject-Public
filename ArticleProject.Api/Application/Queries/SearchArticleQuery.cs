using ArticleProject.Api.Application.Services;
using ArticleProject.Domain.Aggregates;
using ArticleProject.Domain.Common.Exceptions;
using ArticleProject.Domain.Common.Interfaces.Query;
using ArticleProject.Domain.Common.Interfaces.Repository;
using ArticleProject.Model.Models.SearchArticleModels;
using ArticleProject.Resources;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace ArticleProject.Api.Application.Queries
{
    public class SearchArticleQuery : IQuery
    {
        private readonly IReadOnlyRepository<Affiliation> _affiliationRepository;
        private readonly IReadOnlyRepository<Researcher> _researcherRepository;
        private readonly IReadOnlyRepository<Journal> _journalRepository;
        private readonly ScopusService _scopusService;
        private readonly IStringLocalizer<SearchArticleRes> _articleStringLocalizer;
        private readonly IStringLocalizer<AffiliationRes> _affiliationLocalizer;

        public SearchArticleQuery(IReadOnlyRepository<Affiliation> affiliationRepository, IReadOnlyRepository<Researcher> researcherRepository,
                                  IReadOnlyRepository<Journal> journalRepository, ScopusService scopusService, IStringLocalizer<SearchArticleRes> stringLocalizer,
                                  IStringLocalizer<AffiliationRes> affiliationLocalizer)
        {
            _affiliationRepository = affiliationRepository;
            _researcherRepository = researcherRepository;
            _journalRepository = journalRepository;
            _articleStringLocalizer = stringLocalizer;
            _scopusService = scopusService;
            _articleStringLocalizer = stringLocalizer;
            _affiliationLocalizer = affiliationLocalizer;
        }

        public async Task<ArticleDataListModel> GetAllPagedBySearchModelAsync(ArticleSearchInputModel request)
        {
            var response = new ArticleDataListModel();
            var filter = new ArticleSearchModel();

            // If doi is not null, search by DOI
            if (!string.IsNullOrEmpty(request.DOI))
            {
                filter.DOI = request.DOI;
            }
            else
            {
                filter = new ArticleSearchModel
                {
                    Title = request.Title,
                    PublishYear = request.PublishYear,
                    Count = request.PageSize,
                    Index = request.PageIndex * request.PageSize
                };

                if (request.AffiliationId != null)
                {
                    var affiliation = (await _affiliationRepository.GetByIdAsync(request.AffiliationId.Value)).ThrowIfNull(_affiliationLocalizer["ErrorNoAffiliationRecordFound"]);
                    filter.AffiliationScopusId = affiliation?.ScopusId;
                }

                if (request.ResearcherIds != null && request.ResearcherIds.Any())
                {
                    var researcherScopusIds = _researcherRepository.GetAllByFilter(x => request.ResearcherIds.Contains(x.Id)).Select(p => p.ScopusId).ToList();

                    if (researcherScopusIds == null || !researcherScopusIds.Any() || researcherScopusIds.Count != request.ResearcherIds.Count)
                    {
                        throw new NullReferenceException(_articleStringLocalizer["ErrorSomeResearchersNotFound"]);
                    }

                    filter.ResearcherScopusIds = researcherScopusIds;
                }

                if (request.JournalIds != null && request.JournalIds.Any())
                {
                    var journalUniqueIds = _journalRepository.GetAllByFilter(x => request.JournalIds.Contains(x.Id))
                                                                .Select(p => new
                                                                {
                                                                    p.ISSN,
                                                                    p.EISSN
                                                                }).ToList();

                    if (journalUniqueIds == null || !journalUniqueIds.Any() || journalUniqueIds.Count != request.JournalIds.Count)
                    {
                        throw new NullReferenceException(_articleStringLocalizer["ErrorSomeJournalsNotFound"]);
                    }

                    filter.JournalIssnIds = journalUniqueIds.Select(p => p.ISSN).ToList();
                    filter.JournalEissnIds = journalUniqueIds.Select(p => p.EISSN).ToList();
                }
            }

            var articles = await _scopusService.GetArticlesByFilterAsync(filter);

            if (articles.TotalResults == 0)
            {
                response.IsSuccessed = false;
                response.Messages.Add(_articleStringLocalizer["ErrorNoArticlesFound"]);
                return response;
            }

            response.Count = articles.TotalResults;
            response.Data = articles.Entries.Select(p => new ArticleDataModel
            {
                ScopusId = p.ScopusId,
                Eid = p.Eid,
                Title = p.Title,
                Creator = p.Creator,
                PublicationName = p.PublicationName,
                ISSN = p.ISSN,
                EISSN = p.EISSN,
                Volume = p.Volume,
                Issue = p.Issue,
                PageRange = p.PageRange,
                CoverDate = p.CoverDate,
                DOI = p.DOI,
                CitedByCount = p.CitedByCount,
                AffiliationName = p.Affiliations?.FirstOrDefault()?.Name!
            }).ToList();

            var journalISSNIds = response.Data.Select(p => p.ISSN);
            var journalEISSNIds = response.Data.Select(p => p.EISSN);

            var latestJournalYear = _journalRepository.GetAll().Max(p => p.Year);
            var journals = _journalRepository.GetAllByFilter(p => p.Year == latestJournalYear && (journalISSNIds.Contains(p.ISSN) || journalEISSNIds.Contains(p.EISSN)),
                                                i => i.Include(p => p.Category))
                                                .Select(p => new
                                                {
                                                    Id = p.Id,
                                                    ISSN = p.ISSN,
                                                    EISSN = p.EISSN,
                                                    Name = p.Name,
                                                    AbbreviationName = p.AbbreviationName,
                                                    CategoryName = p.Category.Name
                                                }).ToList();

            if (journals != null && journals.Any())
            {
                foreach (var article in response.Data)
                {
                    var journal = journals.FirstOrDefault(p => p.ISSN == article.ISSN || p.EISSN == article.EISSN);

                    if (journal != null)
                    {
                        article.JournalId = journal.Id;
                        article.JournalName = journal.Name;
                        article.JournalAbbreviationName = journal.AbbreviationName;
                        article.CategoryName = journal.CategoryName;
                    }
                }
            }

            response.IsSuccessed = true;
            response.Messages.Add(_articleStringLocalizer["ArticlesFoundSuccessfully"]);
            return response;
        }
    }
}