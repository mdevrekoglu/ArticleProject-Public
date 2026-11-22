using ArticleProject.Api.Application.Services;
using ArticleProject.Domain.Aggregates;
using ArticleProject.Domain.Common.Exceptions;
using ArticleProject.Domain.Common.Extensions;
using ArticleProject.Domain.Common.Interfaces.Query;
using ArticleProject.Domain.Common.Interfaces.Repository;
using ArticleProject.Model.Constants;
using ArticleProject.Model.Models.CommonModels.InputModels;
using ArticleProject.Model.Models.CommonModels.OutputModels;
using ArticleProject.Model.Models.ResearcherModels;
using ArticleProject.Model.Models.SearchArticleModels;
using ArticleProject.Resources;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace ArticleProject.Api.Application.Queries
{
    public class ResearcherQuery : IQuery
    {
        private readonly IReadOnlyRepository<Researcher> _researcherRepository;
        private readonly IReadOnlyRepository<Journal> _journalRepository;
        private readonly ScopusService _scopusService;
        private readonly IStringLocalizer<ResearcherRes> _stringLocalizer;

        public ResearcherQuery(IReadOnlyRepository<Researcher> researcherRepository, IReadOnlyRepository<Journal> journalRepository, ScopusService scopusService, IStringLocalizer<ResearcherRes> stringLocalizer)
        {
            _researcherRepository = researcherRepository;
            _journalRepository = journalRepository;
            _scopusService = scopusService;
            _stringLocalizer = stringLocalizer;
        }

        public async Task<ResearcherDataOutputModel> GetByIdAsync(KeyInputModel request)
        {
            var response = new ResearcherDataOutputModel();

            var researcher = (await _researcherRepository.GetByIdAsync(request.Id, i => i.Include(p => p.Affiliation))).ThrowIfNull(_stringLocalizer["ErrorNoResearcherRecordFound"])!;

            response = new ResearcherDataOutputModel
            {
                Id = researcher.Id,
                FirstName = researcher.FirstName,
                LastName = researcher.LastName,
                Email = researcher.Email,
                DocumentCount = researcher.DocumentCount,
                Position = researcher.Position,
                StaffTitle = researcher.StaffTitle,
                WoSId = researcher.WoSId,
                ORCId = researcher.ORCId,
                ScopusId = researcher.ScopusId,
                IsActive = researcher.IsActive,
                AffiliationId = researcher.AffiliationId,
                AffiliationName = researcher.Affiliation.AffiliationName,
            };

            response.Messages.Add(_stringLocalizer["ResearcherRecordFoundSuccessfully"]);
            response.IsSuccessed = true;

            return response;
        }

        public ResearcherDataListModel GetAllPagedBySearchModel(ResearcherSearchInputModel request)
        {
            var response = new ResearcherDataListModel();

            var expressionFilter = GetAllPagedBySearchModelExpressionFilter(request);

            response.Count = _researcherRepository.GetCountByFilter(expressionFilter);

            if (response.Count == 0)
            {
                response.IsSuccessed = false;
                response.Messages.Add(_stringLocalizer["ErrorNoResearcherRecordFound"]);
                return response;
            }

            response.Data = _researcherRepository.GetAllByFilter(expressionFilter)
                                                    .Select(p => new ResearcherDataModel
                                                    {
                                                        Id = p.Id,
                                                        FirstName = p.FirstName,
                                                        LastName = p.LastName,
                                                        Email = p.Email,
                                                        DocumentCount = p.DocumentCount,
                                                        Position = p.Position,
                                                        StaffTitle = p.StaffTitle,
                                                        WoSId = p.WoSId,
                                                        ORCId = p.ORCId,
                                                        ScopusId = p.ScopusId,
                                                        IsActive = p.IsActive,
                                                        AffiliationId = p.AffiliationId,
                                                        AffiliationName = p.Affiliation.AffiliationName,
                                                    }).OrderBy(request.GetSortingExpression())
                                                    .Skip(request.PageSize * request.PageIndex)
                                                    .Take(request.PageSize)
                                                    .ToList();

            response.IsSuccessed = true;
            response.Messages.Add(_stringLocalizer["ResearcherRecordFoundSuccessfully"]);
            return response;
        }

        private Expression<Func<Researcher, bool>> GetAllPagedBySearchModelExpressionFilter(ResearcherSearchInputModel request)
        {
            Expression<Func<Researcher, bool>> filter = p => true;

            if (!string.IsNullOrWhiteSpace(request.FirstName))
            {
                filter = filter.And(p => p.FirstName.Contains(request.FirstName));
            }

            if (!string.IsNullOrWhiteSpace(request.LastName))
            {
                filter = filter.And(p => p.LastName.Contains(request.LastName));
            }

            if (!string.IsNullOrWhiteSpace(request.Email))
            {
                filter = filter.And(p => p.Email.Contains(request.Email));
            }

            if (!string.IsNullOrWhiteSpace(request.Position))
            {
                filter = filter.And(p => p.Position.Contains(request.Position));
            }

            if (!string.IsNullOrWhiteSpace(request.StaffTitle))
            {
                filter = filter.And(p => p.StaffTitle.Contains(request.StaffTitle));
            }

            if (!string.IsNullOrWhiteSpace(request.ORCId))
            {
                filter = filter.And(p => p.ORCId.Contains(request.ORCId));
            }

            if (!string.IsNullOrWhiteSpace(request.WoSId))
            {
                filter = filter.And(p => p.WoSId.Contains(request.WoSId));
            }

            if (request.ScopusId.HasValue)
            {
                filter = filter.And(p => p.ScopusId == request.ScopusId.Value);
            }

            if (request.AffiliationId.HasValue)
            {
                filter = filter.And(p => p.AffiliationId == request.AffiliationId.Value);
            }

            if (request.IsActive.HasValue)
            {
                filter = filter.And(p => p.IsActive == request.IsActive.Value);
            }

            return filter;
        }

        public List<BaseGetOptionsOutputModel> GetOptionsByKeyword(BaseGetOptionsByKeywordModel request)
        {
            var keywords = request.Keyword.Split(" ", StringSplitOptions.RemoveEmptyEntries);

            var entities = _researcherRepository.GetAll()
                                                .Select(p => new BaseGetOptionsOutputModel
                                                {
                                                    Id = p.Id,
                                                    Name = string.Concat(p.FirstName, " ", p.LastName)
                                                }).Where(p => keywords.All(keyword => p.Name.Contains(keyword)))
                                                .OrderBy(p => p.Name)
                                                .Take(request.OptionsSize)
                                                .ToList();

            return entities;
        }

        public List<BaseGetOptionsOutputModel> GetNotAssignedResearcherOptionsByKeyword(BaseGetOptionsByKeywordModel request)
        {
            var keywords = request.Keyword.Split(" ", StringSplitOptions.RemoveEmptyEntries);

            var entities = _researcherRepository.GetAllByFilter(p => p.User == null, i => i.Include(p => p.User))
                                                .Select(p => new BaseGetOptionsOutputModel
                                                {
                                                    Id = p.Id,
                                                    Name = string.Concat(p.FirstName, " ", p.LastName)
                                                }).Where(p => keywords.All(keyword => p.Name.Contains(keyword)))
                                                .OrderBy(p => p.Name)
                                                .Take(request.OptionsSize)
                                                .ToList();

            return entities;
        }

        public async Task<ResearcherDetailDataOutputModel> GetResearcherDetailsByIdAsync(KeyInputModel request)
        {
            var researcher = (await _researcherRepository.GetByIdAsync(request.Id, i => i.Include(p => p.Affiliation).Include(p => p.User).ThenInclude(p => p.Image))).ThrowIfNull(_stringLocalizer["ErrorNoResearcherRecordFound"])!;

            var articleCount = await _scopusService.GetArticleCountAsync(researcher.ScopusId);

            var articles = await _scopusService.GetArticlesByFilterAsync(new ArticleSearchModel { ResearcherScopusIds = new List<long> { researcher.ScopusId }, Count = articleCount.HasValue && articleCount.Value <= QuerySizeConstants.OptionsLimit ? articleCount.Value : QuerySizeConstants.OptionsLimit });

            var journalISSNIds = articles.Entries.Select(p => p.ISSN).Distinct();
            var journalEISSNIds = articles.Entries.Select(p => p.EISSN).Distinct();

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

            var response = new ResearcherDetailDataOutputModel
            {
                Id = researcher.Id,
                FirstName = researcher.FirstName,
                LastName = researcher.LastName,
                Email = researcher.Email,
                DocumentCount = researcher.DocumentCount,
                Position = researcher.Position,
                StaffTitle = researcher.StaffTitle,
                WoSId = researcher.WoSId,
                ORCId = researcher.ORCId,
                ScopusId = researcher.ScopusId,
                IsActive = researcher.IsActive,
                AffiliationId = researcher.AffiliationId,
                AffiliationName = researcher.Affiliation.AffiliationName,

                UserId = researcher.User?.Id,
                Image = researcher.User?.ImageId != null ? Convert.ToBase64String(researcher.User.Image.Image) : null,

                Articles = articles.Entries.Select(p => new ArticleDataModel
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
                    AffiliationName = p.Affiliations?.FirstOrDefault()?.Name!,
                }).ToList()
            };

            if (journals != null && journals.Any())
            {
                foreach (var article in response.Articles)
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

            response.Messages.Add(_stringLocalizer["ResearcherRecordFoundSuccessfully"]);
            response.IsSuccessed = true;

            return response;
        }
    }
}