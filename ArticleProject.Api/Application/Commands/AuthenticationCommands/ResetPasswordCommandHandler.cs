using ArticleProject.Domain.Aggregates;
using ArticleProject.Model.Models.CommonModels.OutputModels;
using ArticleProject.Resources;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;

namespace ArticleProject.Api.Application.Commands.AuthenticationCommands
{
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, MessageOutputModel>
    {
        private readonly UserManager<User> _userManager;
        private readonly IStringLocalizer<AuthenticationRes> _stringLocalizer;

        public ResetPasswordCommandHandler(UserManager<User> userManager, IStringLocalizer<AuthenticationRes> stringLocalizer)
        {
            _userManager = userManager;
            _stringLocalizer = stringLocalizer;
        }

        public async Task<MessageOutputModel> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var response = new MessageOutputModel();

            var user = await _userManager.FindByEmailAsync(request.Model.Email!);

            if (user == null)
            {
                response.IsSuccessed = false;
                response.Messages.Add(_stringLocalizer["ErrorUnknown"]);
                return response;
            }

            var result = await _userManager.ResetPasswordAsync(user, request.Model.Token!, request.Model.NewPassword!);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(p => p.Description).ToList();
                response.IsSuccessed = false;
                response.Messages.AddRange(errors);
                return response;
            }

            await _userManager.SetLockoutEndDateAsync(user, null);

            response.IsSuccessed = true;
            response.Messages.Add(_stringLocalizer["PasswordResettedSuccessfully"]);
            return response;
        }
    }
}