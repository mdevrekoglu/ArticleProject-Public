using ArticleProject.Domain.Aggregates;
using ArticleProject.Domain.Common.Exceptions;
using ArticleProject.Domain.Common.Extensions;
using ArticleProject.Domain.Common.Interfaces.Query;
using ArticleProject.Domain.Common.Interfaces.Repository;
using ArticleProject.Model.Models.CommonModels.InputModels;
using ArticleProject.Model.Models.CommonModels.OutputModels;
using ArticleProject.Model.Models.DepartmentModels;
using ArticleProject.Resources;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace ArticleProject.Api.Application.Queries
{
    public class DepartmentQuery : IQuery
    {
        private readonly IReadOnlyRepository<Department> _departmentRepository;
        private readonly IStringLocalizer<DepartmentRes> _stringLocalizer;

        public DepartmentQuery(IReadOnlyRepository<Department> departmentRepository, IStringLocalizer<DepartmentRes> stringLocalizer)
        {
            _departmentRepository = departmentRepository;
            _stringLocalizer = stringLocalizer;
        }

        public async Task<DepartmentDataOutputModel> GetByIdAsync(KeyInputModel request)
        {
            var response = new DepartmentDataOutputModel();

            var entity = (await _departmentRepository.GetByIdAsync(request.Id, i => i.Include(p => p.Faculty).ThenInclude(f => f.Affiliation))).ThrowIfNull(_stringLocalizer["ErrorNoDepartmentRecordFound"])!;

            response = new DepartmentDataOutputModel
            {
                Id = entity.Id,
                DepartmentName = entity.DepartmentName,
                StudentCount = entity.StudentCount,
                ResearcherCount = entity.ResearcherCount,
                FacultyId = entity.FacultyId,
                FacultyName = entity.Faculty.FacultyName,
                AffiliationId = entity.Faculty.Affiliation.Id,
                AffiliationName = entity.Faculty.Affiliation.AffiliationName,
            };

            response.Messages.Add(_stringLocalizer["DepartmentRecordFoundSuccessfully"]);
            response.IsSuccessed = true;

            return response;
        }

        public List<BaseGetOptionsOutputModel> GetOptionsByKeyword(DepartmentGetOptionsByKeywordInputModel request)
        {
            var entities = _departmentRepository.GetAllByFilter(p => p.FacultyId == request.FacultyId && p.DepartmentName.ToUpper().Contains(request.Keyword.ToUpper()))
                                                                    .Select(p => new BaseGetOptionsOutputModel
                                                                    {
                                                                        Id = p.Id,
                                                                        Name = p.DepartmentName
                                                                    }).OrderBy(p => p.Name).Take(request.OptionsSize).ToList();

            return entities;
        }

        public DepartmentDataListModel GetAllPagedBySearchModel(DepartmentSearchInputModel request)
        {
            var response = new DepartmentDataListModel();

            var expressionFilter = GetExpressionFilter(request);

            response.Count = _departmentRepository.GetCountByFilter(expressionFilter);

            if (response.Count == 0)
            {
                response.IsSuccessed = false;
                response.Messages.Add(_stringLocalizer["ErrorNoDepartmentRecordFound"]);
                return response;
            }

            response.Data = _departmentRepository.GetAllByFilter(expressionFilter, i => i.Include(p => p.Faculty).ThenInclude(f => f.Affiliation))
                                                .Select(p => new DepartmentDataModel
                                                {
                                                    Id = p.Id,
                                                    DepartmentName = p.DepartmentName,
                                                    StudentCount = p.StudentCount,
                                                    ResearcherCount = p.ResearcherCount,
                                                    FacultyId = p.FacultyId,
                                                    FacultyName = p.Faculty.FacultyName,
                                                    AffiliationId = p.Faculty.Affiliation.Id,
                                                    AffiliationName = p.Faculty.Affiliation.AffiliationName,
                                                }).OrderBy(request.GetSortingExpression())
                                              .Skip(request.PageSize * request.PageIndex)
                                              .Take(request.PageSize)
                                              .ToList();

            response.IsSuccessed = true;
            response.Messages.Add(_stringLocalizer["DepartmentRecordFoundSuccessfully"]);
            return response;
        }

        private Expression<Func<Department, bool>> GetExpressionFilter(DepartmentSearchInputModel request)
        {
            Expression<Func<Department, bool>> filter = p => true;

            if (!string.IsNullOrWhiteSpace(request.DepartmentName))
            {
                filter = filter.And(p => p.DepartmentName.Contains(request.DepartmentName));
            }

            if (request.AffiliationId.HasValue)
            {
                filter = filter.And(p => request.AffiliationId == p.Faculty.AffiliationId);
            }

            if (request.FacultyId.HasValue)
            {
                filter = filter.And(p => request.FacultyId == p.FacultyId);
            }

            return filter;
        }
    }
}