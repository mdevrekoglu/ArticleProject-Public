using ArticleProject.Domain.Aggregates;
using ArticleProject.Domain.Common.Extensions;
using ArticleProject.Domain.Common.Interfaces.Query;
using ArticleProject.Model.Models.CommonModels.InputModels;
using ArticleProject.Model.Models.CommonModels.OutputModels;
using ArticleProject.Model.Models.UserManagementModels;
using ArticleProject.Resources;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System.Data;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace ArticleProject.Api.Application.Queries
{
    public class UserManagementQuery : IQuery
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IStringLocalizer<UserManagementRes> _stringLocalizer;

        public UserManagementQuery(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IStringLocalizer<UserManagementRes> stringLocalizer)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _stringLocalizer = stringLocalizer;
        }

        // TODO Mehmet queryler düzgün çalışmıyor.
        public async Task<UserDataOutputModel> GetByIdAsync(KeyStringInputModel request)
        {
            var userQuery = _userManager.Users.AsNoTracking()
                                              .Include(p => p.Department).ThenInclude(p => p.Faculty).ThenInclude(p => p.Affiliation)
                                              .Include(p => p.UserRoles);

            var roleQuery = _roleManager.Roles.AsNoTracking();

            // Objenin navigation propery'si olmadığı için eski usül query yazıldı
            var response = await (from user in userQuery
                                  from userRole in user.UserRoles
                                  join role in roleQuery
                                  on userRole.RoleId equals role.Id
                                  group role by user into userGroup
                                  select new UserDataOutputModel
                                  {
                                      Id = userGroup.Key.Id,
                                      FirstName = userGroup.Key.FirstName,
                                      LastName = userGroup.Key.LastName,
                                      Email = userGroup.Key.Email,
                                      DepartmentId = userGroup.Key.DepartmentId,
                                      DepartmentName = userGroup.Key.Department.DepartmentName,
                                      FacultyName = userGroup.Key.Department.Faculty.FacultyName,
                                      AffiliationName = userGroup.Key.Department.Faculty.Affiliation.AffiliationName,
                                      ResearcherId = userGroup.Key.ResearcherId,
                                      Roles = userGroup.Select(p => new UserRoleModel
                                      {
                                          Id = p.Id,
                                          Name = p.Name
                                      }).ToList(),
                                  }).FirstOrDefaultAsync(p => p.Id.Equals(request.Id));

            if (response == null)
                throw new NullReferenceException(_stringLocalizer["ErrorNoUserFound"]);

            response.IsSuccessed = true;
            response.Messages.Add(_stringLocalizer["UsersFoundSuccessfully"]);
            return response;
        }

        public UserDataListModel GetAllPagedBySearchModel(UserSearchInputModel request)
        {
            var response = new UserDataListModel();

            var userQuery = _userManager.Users.AsNoTracking()
                                              .Include(p => p.Department).ThenInclude(p => p.Faculty).ThenInclude(p => p.Affiliation)
                                              .Include(p => p.UserRoles)
                                              .Where(GetExpressionFilter(request));

            var roleQuery = _roleManager.Roles.AsNoTracking();

            // Objenin navigation propery'si olmadığı için eski usül query yazıldı
            var resultQuery = from user in userQuery
                              from userRole in user.UserRoles
                              join role in roleQuery
                              on userRole.RoleId equals role.Id
                              group role by user into userGroup
                              // Role hariç bütün filtreler uygulandı.
                              where string.IsNullOrWhiteSpace(request.Role) || userGroup.Select(v => v.NormalizedName).Contains(request.Role)
                              select new UserDataModel
                              {
                                  Id = userGroup.Key.Id,
                                  FirstName = userGroup.Key.FirstName,
                                  LastName = userGroup.Key.LastName,
                                  Email = userGroup.Key.Email,
                                  DepartmentId = userGroup.Key.DepartmentId,
                                  DepartmentName = userGroup.Key.Department.DepartmentName,
                                  FacultyName = userGroup.Key.Department.Faculty.FacultyName,
                                  AffiliationName = userGroup.Key.Department.Faculty.Affiliation.AffiliationName,
                                  ResearcherId = userGroup.Key.ResearcherId,
                                  Roles = userGroup.Select(p => p.Name).ToList(),
                              };

            response.Count = resultQuery.Count();
            if (response.Count == 0)
            {
                response.IsSuccessed = false;
                response.Messages.Add(_stringLocalizer["ErrorNoUserFound"]);
                return response;
            }

            response.Data = resultQuery.OrderBy(request.GetSortingExpression())
                                       .Skip(request.PageSize * request.PageIndex)
                                       .Take(request.PageSize)
                                       .ToList();

            response.IsSuccessed = true;
            response.Messages.Add(_stringLocalizer["UsersFoundSuccessfully"]);
            return response;
        }

        private Expression<Func<User, bool>> GetExpressionFilter(UserSearchInputModel request)
        {
            Expression<Func<User, bool>> filter = p => true;

            if (!string.IsNullOrWhiteSpace(request.FirstName))
            {
                filter = filter.And(p => p.FirstName.Contains(request.FirstName));
            }

            if (!string.IsNullOrWhiteSpace(request.LastName))
            {
                filter = filter.And(p => p.LastName.Contains(request.LastName));
            }

            // Start with the most informative field
            if (request.DepartmentId.HasValue)
            {
                filter = filter.And(p => p.DepartmentId == request.DepartmentId.Value);
            }
            else if (request.FacultyId.HasValue)
            {
                filter = filter.And(p => p.DepartmentId != null && p.Department.FacultyId == request.FacultyId.Value);
            }
            else if (request.AffiliationId.HasValue)
            {
                filter = filter.And(p => p.DepartmentId != null && p.Department.Faculty.AffiliationId == request.AffiliationId.Value);
            }

            return filter;
        }

        public List<BaseGetOptionsStringOutputModel> GetOptionsBySearchModel(UserOptionsSearchInputModel request)
        {
            var response = _userManager.Users.AsNoTracking()
                                              .Include(p => p.Department).ThenInclude(p => p.Faculty).ThenInclude(p => p.Affiliation)
                                              .Where(GetAllOptionsExpressionFilter(request))
                                              .Select(p => new BaseGetOptionsStringOutputModel
                                              {
                                                  Id = p.Id,
                                                  Name = string.Concat(p.FirstName, " ", p.LastName)
                                              }).OrderBy(p => p.Name)
                                              .Take(request.OptionsSize)
                                              .ToList();

            return response;
        }

        public List<BaseGetOptionsStringOutputModel> GetOptionsByKeyword(BaseGetOptionsByKeywordModel request)
        {
            var response = _userManager.Users.AsNoTracking()
                                             .Select(p => new BaseGetOptionsStringOutputModel
                                             {
                                                 Id = p.Id,
                                                 Name = string.Concat(p.FirstName, " ", p.LastName)
                                             }).Where(p => p.Name.Contains(request.Keyword))
                                             .OrderBy(p => p.Name)
                                             .Take(request.OptionsSize)
                                             .ToList();

            return response;
        }

        private Expression<Func<User, bool>> GetAllOptionsExpressionFilter(UserOptionsSearchInputModel request)
        {
            Expression<Func<User, bool>> filter = p => true;

            // Start with the most informative field
            if (request.DepartmentId.HasValue)
            {
                filter = filter.And(p => p.DepartmentId == request.DepartmentId.Value);
            }
            else if (request.FacultyId.HasValue)
            {
                filter = filter.And(p => p.DepartmentId != null && p.Department.FacultyId == request.FacultyId.Value);
            }
            else if (request.AffiliationId.HasValue)
            {
                filter = filter.And(p => p.DepartmentId != null && p.Department.Faculty.AffiliationId == request.AffiliationId.Value);
            }

            return filter;
        }
    }
}