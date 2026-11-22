using ArticleProject.Domain.Aggregates;
using ArticleProject.Model.Models.CommonModels.OutputModels;
using ArticleProject.Resources;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace ArticleProject.Api.Application.Commands.UserManagementCommands
{
    public class AssignUserResearcherCommandHandler : IRequestHandler<AssignUserResearcherCommand, MessageOutputModel>
    {
        private readonly UserManager<User> _userManager;
        private readonly IStringLocalizer<UserManagementRes> _stringLocalizer;

        public AssignUserResearcherCommandHandler(UserManager<User> userManager, IStringLocalizer<UserManagementRes> stringLocalizer)
        {
            _userManager = userManager;
            _stringLocalizer = stringLocalizer;
        }

        public async Task<MessageOutputModel> Handle(AssignUserResearcherCommand request, CancellationToken cancellationToken)
        {
            var response = new MessageOutputModel();

            var user = await _userManager.Users.Include(p => p.Researcher)
                                               .Include(p => p.Department).ThenInclude(p => p.Faculty).ThenInclude(p => p.Affiliation)
                                               .FirstOrDefaultAsync(p => p.Id.Equals(request.Model.UserId));

            if (user == null)
                throw new NullReferenceException(_stringLocalizer["ErrorNoUserFound"]);

            user.UpdateResearcher(request.Model.ResearcherId);

            await _userManager.UpdateAsync(user);

            response.IsSuccessed = true;
            response.Messages.Add(_stringLocalizer["UserAssignedToResearcherSuccessfully"]);
            return response;
        }
    }
}