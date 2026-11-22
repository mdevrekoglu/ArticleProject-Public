using ArticleProject.Domain.Aggregates;
using ArticleProject.Domain.Common.Exceptions;
using ArticleProject.Domain.Common.Extensions;
using ArticleProject.Domain.Common.Interfaces.Query;
using ArticleProject.Domain.Common.Interfaces.Repository;
using ArticleProject.Model.Models.CommonModels.InputModels;
using ArticleProject.Model.Models.MajorDisciplineModels;
using ArticleProject.Resources;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace ArticleProject.Api.Application.Queries
{
    public class MajorDisciplineQuery : IQuery
    {
        private readonly IReadOnlyRepository<MajorDiscipline> _majorDisciplineRepository;
        private readonly IStringLocalizer<MajorDisciplineRes> _stringLocalizer;

        public MajorDisciplineQuery(IReadOnlyRepository<MajorDiscipline> majorDisciplineRepository, IStringLocalizer<MajorDisciplineRes> stringLocalizer)
        {
            _majorDisciplineRepository = majorDisciplineRepository;
            _stringLocalizer = stringLocalizer;
        }

        public async Task<MajorDisciplineDataOutputModel> GetByIdAsync(KeyInputModel request)
        {
            var response = new MajorDisciplineDataOutputModel();

            var entity = (await _majorDisciplineRepository.GetByIdAsync(request.Id, i => i.Include(p => p.Department).ThenInclude(f => f.Faculty).ThenInclude(a => a.Affiliation))).ThrowIfNull(_stringLocalizer["ErrorNoMajorDisciplineRecordFound"])!;

            response = new MajorDisciplineDataOutputModel
            {
                Id = entity.Id,
                MajorDisciplineName = entity.MajorDisciplineName,
                DepartmentId = entity.DepartmentId,
                DepartmentName = entity.Department.DepartmentName,
                FacultyId = entity.Department.FacultyId,
                FacultyName = entity.Department.Faculty.FacultyName,
                AffiliationId = entity.Department.Faculty.AffiliationId,
                AffiliationName = entity.Department.Faculty.Affiliation.AffiliationName,
            };

            response.Messages.Add(_stringLocalizer["MajorDisciplineRecordFoundSuccessfully"]);
            response.IsSuccessed = true;

            return response;
        }

        public MajorDisciplineDataListModel GetAllPagedBySearchModel(MajorDisciplineSearchInputModel request)
        {
            var response = new MajorDisciplineDataListModel();

            var expressionFilter = GetExpressionFilter(request);

            response.Count = _majorDisciplineRepository.GetCountByFilter(expressionFilter);

            if (response.Count == 0)
            {
                response.IsSuccessed = false;
                response.Messages.Add(_stringLocalizer["ErrorNoMajorDisciplineRecordFound"]);
                return response;
            }

            response.Data = _majorDisciplineRepository.GetAllByFilter(expressionFilter, i =>
                                                        i.Include(p => p.Department).ThenInclude(p => p.Faculty).ThenInclude(f => f.Affiliation))
                                                        .Select(p => new MajorDisciplineDataModel
                                                        {
                                                            Id = p.Id,
                                                            MajorDisciplineName = p.MajorDisciplineName,
                                                            DepartmentId = p.DepartmentId,
                                                            DepartmentName = p.Department.DepartmentName,
                                                            FacultyId = p.Department.FacultyId,
                                                            FacultyName = p.Department.Faculty.FacultyName,
                                                            AffiliationId = p.Department.Faculty.AffiliationId,
                                                            AffiliationName = p.Department.Faculty.Affiliation.AffiliationName,
                                                        }).OrderBy(request.GetSortingExpression())
                                                        .Skip(request.PageSize * request.PageIndex)
                                                        .Take(request.PageSize)
                                                        .ToList();

            response.IsSuccessed = true;
            response.Messages.Add(_stringLocalizer["MajorDisciplineRecordFoundSuccessfully"]);
            return response;
        }

        private Expression<Func<MajorDiscipline, bool>> GetExpressionFilter(MajorDisciplineSearchInputModel request)
        {
            Expression<Func<MajorDiscipline, bool>> filter = p => true;

            if (!string.IsNullOrWhiteSpace(request.MajorDisciplineName))
            {
                filter = filter.And(p => p.MajorDisciplineName.Contains(request.MajorDisciplineName));
            }

            if (request.DepartmentIds != null && request.DepartmentIds.Any())
            {
                filter = filter.And(p => request.DepartmentIds.Contains(p.DepartmentId));
            }

            if (request.FacultyIds != null && request.FacultyIds.Any())
            {
                filter = filter.And(p => request.FacultyIds.Contains(p.Department.Faculty.Id));
            }

            if (request.AffiliationIds != null && request.AffiliationIds.Any())
            {
                filter = filter.And(p => request.AffiliationIds.Contains(p.Department.Faculty.Affiliation.Id));
            }

            return filter;
        }
    }
}