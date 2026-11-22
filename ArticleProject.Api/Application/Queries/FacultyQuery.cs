using ArticleProject.Domain.Aggregates;
using ArticleProject.Domain.Common.Exceptions;
using ArticleProject.Domain.Common.Extensions;
using ArticleProject.Domain.Common.Interfaces.Query;
using ArticleProject.Domain.Common.Interfaces.Repository;
using ArticleProject.Model.Models.CommonModels.InputModels;
using ArticleProject.Model.Models.CommonModels.OutputModels;
using ArticleProject.Model.Models.FacultyModels;
using ArticleProject.Resources;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace ArticleProject.Api.Application.Queries
{
    public class FacultyQuery : IQuery
    {
        private readonly IReadOnlyRepository<Faculty> _facultyRepository;
        private readonly IStringLocalizer<FacultyRes> _stringLocalizer;

        public FacultyQuery(IReadOnlyRepository<Faculty> facultyRepository, IStringLocalizer<FacultyRes> stringLocalizer)
        {
            _facultyRepository = facultyRepository;
            _stringLocalizer = stringLocalizer;
        }

        public async Task<FacultyDataOutputModel> GetByIdAsync(KeyInputModel request)
        {
            var response = new FacultyDataOutputModel();

            var entity = (await _facultyRepository.GetByIdAsync(request.Id, i => i.Include(p => p.Affiliation))).ThrowIfNull(_stringLocalizer["ErrorNoFacultyRecordFound"])!;

            response = new FacultyDataOutputModel
            {
                Id = entity.Id,
                FacultyName = entity.FacultyName,
                AffiliationId = entity.AffiliationId,
                AffiliationName = entity.Affiliation.AffiliationName
            };

            response.Messages.Add(_stringLocalizer["FacultyRecordFoundSuccessfully"]);
            response.IsSuccessed = true;

            return response;
        }

        public FacultyDataListModel GetAllPagedBySearchModel(FacultySearchInputModel request)
        {
            var response = new FacultyDataListModel();

            var expressionFilter = GetExpressionFilter(request);

            response.Count = _facultyRepository.GetCountByFilter(expressionFilter);

            if (response.Count == 0)
            {
                response.IsSuccessed = false;
                response.Messages.Add(_stringLocalizer["ErrorNoFacultyRecordFound"]);
                return response;
            }

            response.Data = _facultyRepository.GetAllByFilter(expressionFilter, i => i.Include(p => p.Affiliation))
                                                .Select(p => new FacultyDataModel
                                                {
                                                    Id = p.Id,
                                                    FacultyName = p.FacultyName,
                                                    AffiliationId = p.AffiliationId,
                                                    AffiliationName = p.Affiliation.AffiliationName
                                                }).OrderBy(request.GetSortingExpression())
                                                .Skip(request.PageSize * request.PageIndex)
                                                .Take(request.PageSize)
                                                .ToList();

            response.IsSuccessed = true;
            response.Messages.Add(_stringLocalizer["FacultyRecordFoundSuccessfully"]);
            return response;
        }

        private Expression<Func<Faculty, bool>> GetExpressionFilter(FacultySearchInputModel request)
        {
            Expression<Func<Faculty, bool>> filter = p => true;

            if (!string.IsNullOrWhiteSpace(request.FacultyName))
            {
                filter = filter.And(p => p.FacultyName.Contains(request.FacultyName));
            }

            if (request.AffiliationIds != null && request.AffiliationIds.Any())
            {
                filter = filter.And(p => request.AffiliationIds.Contains(p.AffiliationId));
            }

            return filter;
        }

        public List<BaseGetOptionsOutputModel> GetOptionsByKeyword(FacultyGetOptionsByKeywordInputModel request)
        {
            var entities = _facultyRepository.GetAllByFilter(p => p.AffiliationId == request.AffiliationId && p.FacultyName.ToUpper().Contains(request.Keyword.ToUpper()))
                                                                    .Select(p => new BaseGetOptionsOutputModel
                                                                    {
                                                                        Id = p.Id,
                                                                        Name = p.FacultyName
                                                                    }).OrderBy(p => p.Name).Take(request.OptionsSize).ToList();

            return entities;
        }
    }
}