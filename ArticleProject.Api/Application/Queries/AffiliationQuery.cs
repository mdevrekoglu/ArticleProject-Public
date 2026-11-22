using ArticleProject.Domain.Aggregates;
using ArticleProject.Domain.Common.Exceptions;
using ArticleProject.Domain.Common.Extensions;
using ArticleProject.Domain.Common.Interfaces.Query;
using ArticleProject.Domain.Common.Interfaces.Repository;
using ArticleProject.Model.Models.AffiliatonModels;
using ArticleProject.Model.Models.CommonModels.InputModels;
using ArticleProject.Model.Models.CommonModels.OutputModels;
using ArticleProject.Resources;
using Microsoft.Extensions.Localization;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace ArticleProject.Api.Application.Queries
{
    public class AffiliationQuery : IQuery
    {
        private readonly IReadOnlyRepository<Affiliation> _affiliationRepository;
        private readonly IStringLocalizer<AffiliationRes> _stringLocalizer;

        public AffiliationQuery(IReadOnlyRepository<Affiliation> affiliationRepository, IStringLocalizer<AffiliationRes> stringLocalizer)
        {
            _affiliationRepository = affiliationRepository;
            _stringLocalizer = stringLocalizer;
        }

        public async Task<AffiliationDataOutputModel> GetByIdAsync(KeyInputModel request)
        {
            var response = new AffiliationDataOutputModel();

            var entity = (await _affiliationRepository.GetByIdAsync(request.Id)).ThrowIfNull(_stringLocalizer["ErrorNoAffiliationRecordFound"])!;

            response = new AffiliationDataOutputModel
            {
                Id = entity.Id,
                Address = entity.Address,
                City = entity.City,
                Country = entity.Country,
                AffiliationName = entity.AffiliationName,
                ScopusId = entity.ScopusId,
                OrganizationType = entity.OrganizationType,
            };

            response.Messages.Add(_stringLocalizer["AffiliationRecordFoundSuccessfully"]);
            response.IsSuccessed = true;

            return response;
        }

        public AffiliationDataListModel GetAllPagedBySearchModel(AffiliationSearchInputModel request)
        {
            var response = new AffiliationDataListModel();

            var expressionFilter = GetExpressionFilter(request);

            response.Count = _affiliationRepository.GetCountByFilter(expressionFilter);

            if (response.Count == 0)
            {
                response.IsSuccessed = false;
                response.Messages.Add(_stringLocalizer["ErrorNoAffiliationRecordFound"]);
                return response;
            }

            response.Data = _affiliationRepository.GetAllByFilter(expressionFilter)
                                                    .Select(p => new AffiliationDataModel
                                                    {
                                                        Id = p.Id,
                                                        Address = p.Address,
                                                        AffiliationName = p.AffiliationName,
                                                        City = p.City,
                                                        OrganizationType = p.OrganizationType,
                                                        ScopusId = p.ScopusId,
                                                        Country = p.Country,
                                                    }).OrderBy(request.GetSortingExpression())
                                                    .Skip(request.PageSize * request.PageIndex)
                                                    .Take(request.PageSize)
                                                    .ToList();

            response.IsSuccessed = true;
            response.Messages.Add(_stringLocalizer["AffiliationRecordFoundSuccessfully"]);
            return response;
        }

        private Expression<Func<Affiliation, bool>> GetExpressionFilter(AffiliationSearchInputModel request)
        {
            Expression<Func<Affiliation, bool>> filter = p => true;

            if (!string.IsNullOrWhiteSpace(request.AffiliationName))
            {
                filter = filter.And(p => p.AffiliationName.Contains(request.AffiliationName));
            }

            if (!string.IsNullOrWhiteSpace(request.Address))
            {
                filter = filter.And(p => p.Address.Contains(request.Address));
            }

            if (!string.IsNullOrWhiteSpace(request.Country))
            {
                filter = filter.And(p => p.Country.Contains(request.Country));
            }

            if (!string.IsNullOrWhiteSpace(request.City))
            {
                filter = filter.And(p => p.City.Contains(request.City));
            }

            if (request.ScopusId.HasValue)
            {
                filter = filter.And(p => p.ScopusId == request.ScopusId.Value);
            }

            if (!string.IsNullOrWhiteSpace(request.OrganizationType))
            {
                filter = filter.And(p => p.OrganizationType.Contains(request.OrganizationType));
            }

            return filter;
        }

        public List<BaseGetOptionsOutputModel> GetOptionsByKeyword(BaseGetOptionsByKeywordModel request)
        {
            var entities = _affiliationRepository.GetAllByFilter(p => p.AffiliationName.Contains(request.Keyword))
                                                    .Select(p => new BaseGetOptionsOutputModel
                                                    {
                                                        Id = p.Id,
                                                        Name = p.AffiliationName
                                                    }).OrderBy(p => p.Name).Take(request.OptionsSize).ToList();

            return entities;
        }
    }
}