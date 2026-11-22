using ArticleProject.Domain.Aggregates;
using ArticleProject.Model.Models.UserManagementModels;
using ArticleProject.Resources;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace ArticleProject.Api.Application.Commands.UserManagementCommands
{
    public class UpdateUserRolesCommandHandler : IRequestHandler<UpdateUserRolesCommand, UserDataOutputModel>
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IStringLocalizer<UserManagementRes> _stringLocalizer;

        public UpdateUserRolesCommandHandler(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IStringLocalizer<UserManagementRes> stringLocalizer)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _stringLocalizer = stringLocalizer;
        }

        public async Task<UserDataOutputModel> Handle(UpdateUserRolesCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.Users.Include(p => p.UserRoles)
                                               .Include(p => p.Researcher)
                                               .Include(p => p.Department).ThenInclude(p => p.Faculty).ThenInclude(p => p.Affiliation)
                                               .FirstOrDefaultAsync(p => p.Id.Equals(request.Model.UserId));

            if (user == null)
                throw new NullReferenceException(_stringLocalizer["ErrorNoUserFound"]);

            var currentRoles = (await _userManager.GetRolesAsync(user)).ToList();
            var newRoles = _roleManager.Roles.Where(p => request.Model.RoleIds.Contains(p.Id)).ToList();

            if (newRoles.Count() != request.Model.RoleIds.Count())
                throw new NullReferenceException(_stringLocalizer["ErrorSomeRolesNotFound"]);

            var newRoleNames = newRoles.Select(p => p.Name);

            // Roles to remove
            var rolesToRemove = currentRoles.Except(newRoleNames);
            // Roles to add
            var rolesToAdd = newRoleNames.Except(currentRoles);

            if (rolesToRemove != null && rolesToRemove.Any())
            {
                await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
            }
            if (rolesToAdd != null && rolesToAdd.Any())
            {
                await _userManager.AddToRolesAsync(user, rolesToAdd);
            }

            var response = new UserDataOutputModel
            {
                Id = user.Id,
                Email = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Roles = newRoles.Select(p => new UserRoleModel
                {
                    Id = p.Id,
                    Name = p.Name
                }).ToList(),
                ResearcherId = user.ResearcherId,
                DepartmentId = user.DepartmentId,
                DepartmentName = user.Department?.DepartmentName,
                FacultyName = user.Department?.Faculty.FacultyName,
                AffiliationName = user.Department?.Faculty.Affiliation.AffiliationName
            };

            response.IsSuccessed = true;
            response.Messages.Add(_stringLocalizer["UserRolesUpdatedSuccessfully"]);
            return response;
        }
    }
}