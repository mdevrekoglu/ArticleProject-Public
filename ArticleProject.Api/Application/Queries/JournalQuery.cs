using ArticleProject.Domain.Aggregates;
using ArticleProject.Domain.Common.Exceptions;
using ArticleProject.Domain.Common.Extensions;
using ArticleProject.Domain.Common.Interfaces.Query;
using ArticleProject.Domain.Common.Interfaces.Repository;
using ArticleProject.Model.Models.CommonModels.InputModels;
using ArticleProject.Model.Models.CommonModels.OutputModels;
using ArticleProject.Model.Models.JournalModels;
using ArticleProject.Resources;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace ArticleProject.Api.Application.Queries
{
    public class JournalQuery : IQuery
    {
        private readonly IReadOnlyRepository<Journal> _journalRepository;
        private readonly IStringLocalizer<JournalRes> _stringLocalizer;

        public JournalQuery(IReadOnlyRepository<Journal> journalRepository, IStringLocalizer<JournalRes> stringLocalizer)
        {
            _journalRepository = journalRepository;
            _stringLocalizer = stringLocalizer;
        }

        public async Task<JournalDataOutputModel> GetByIdAsync(KeyInputModel request)
        {
            var response = new JournalDataOutputModel();

            var entity = (await _journalRepository.GetByIdAsync(request.Id, i => i.Include(p => p.Category))).ThrowIfNull(_stringLocalizer["ErrorNoAffiliationRecordFound"])!;

            response = new JournalDataOutputModel
            {
                Id = entity.Id,
                Name = entity.Name,
                AbbreviationName = entity.AbbreviationName,
                ISSN = entity.ISSN,
                EISSN = entity.EISSN,
                JIF = entity.JIF,
                JIF5Years = entity.JIF5Years,
                CategoryId = entity.CategoryId,
                CategoryName = entity.Category.Name,
                Quartile = entity.Category.Quartile,
                Rank = entity.Category.Rank,
                TotalJournals = entity.Category.TotalJournals,
                Year = entity.Year,
            };

            response.Messages.Add(_stringLocalizer["ResearcherRecordFoundSuccessfully"]);
            response.IsSuccessed = true;

            return response;
        }

        public JournalDataListModel GetAllPagedBySearchModel(JournalSearchInputModel request)
        {
            var response = new JournalDataListModel();

            var expressionFilter = GetAllPagedBySearchModelExpressionFilter(request);

            response.Count = _journalRepository.GetCountByFilter(expressionFilter);

            if (response.Count == 0)
            {
                response.IsSuccessed = false;
                response.Messages.Add(_stringLocalizer["ErrorNoJournalRecordFound"]);
                return response;
            }

            response.Data = _journalRepository.GetAllByFilter(expressionFilter)
                                                   .Select(p => new JournalDataModel
                                                   {
                                                       Id = p.Id,
                                                       Name = p.Name,
                                                       AbbreviationName = p.AbbreviationName,
                                                       ISSN = p.ISSN,
                                                       EISSN = p.EISSN,
                                                       JIF = p.JIF,
                                                       JIF5Years = p.JIF5Years,
                                                       CategoryId = p.CategoryId,
                                                       CategoryName = p.Category.Name,
                                                       Quartile = p.Category.Quartile,
                                                       Rank = p.Category.Rank,
                                                       TotalJournals = p.Category.TotalJournals,
                                                       Year = p.Year,
                                                   }).OrderBy(request.GetSortingExpression())
                                                   .Skip(request.PageSize * request.PageIndex)
                                                   .Take(request.PageSize)
                                                   .ToList();

            response.IsSuccessed = true;
            response.Messages.Add(_stringLocalizer["JournalRecordFoundSuccessfully"]);
            return response;
        }

        private Expression<Func<Journal, bool>> GetAllPagedBySearchModelExpressionFilter(JournalSearchInputModel request)
        {
            Expression<Func<Journal, bool>> filter = p => true;

            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                filter = filter.And(p => p.Name.Contains(request.Name));
            }

            if (!string.IsNullOrWhiteSpace(request.AbbreviationName))
            {
                filter = filter.And(p => p.AbbreviationName.Contains(request.AbbreviationName));
            }

            if (!string.IsNullOrWhiteSpace(request.ISSN))
            {
                filter = filter.And(p => p.ISSN.Contains(request.ISSN));
            }

            if (!string.IsNullOrWhiteSpace(request.EISSN))
            {
                filter = filter.And(p => p.EISSN.Contains(request.EISSN));
            }

            if (request.Quartile.HasValue)
            {
                filter = filter.And(p => p.Category.Quartile == request.Quartile);
            }

            if (!string.IsNullOrWhiteSpace(request.CategoryName))
            {
                filter = filter.And(p => p.Category.Name == request.CategoryName);
            }

            if (request.Quartile.HasValue)
            {
                filter = filter.And(p => p.Category.Quartile == request.Quartile);
            }

            if (request.Rank.HasValue)
            {
                filter = filter.And(p => p.Category.Rank == request.Rank);
            }

            if (request.TotalJournals.HasValue)
            {
                filter = filter.And(p => p.Category.TotalJournals == request.TotalJournals);
            }

            if (request.Year.HasValue)
            {
                filter = filter.And(p => p.Year == request.Year);
            }

            return filter;
        }

        public List<BaseGetOptionsOutputModel> GetOptionsByKeyword(BaseGetOptionsByKeywordModel request)
        {
            var latestYear = _journalRepository.GetAll().Max(p => p.Year);

            var entities = _journalRepository.GetAllByFilter(p => !string.IsNullOrEmpty(p.ISSN) && p.Year == latestYear && p.Name.Contains(request.Keyword))
                                                    .Select(p => new BaseGetOptionsOutputModel
                                                    {
                                                        Id = p.Id,
                                                        Name = p.AbbreviationName
                                                    }).OrderBy(p => p.Name)
                                                    .Take(request.OptionsSize)
                                                    .ToList();

            return entities;
        }
    }
}