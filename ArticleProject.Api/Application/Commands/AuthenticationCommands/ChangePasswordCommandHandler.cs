using ArticleProject.Domain.Aggregates;
using ArticleProject.Model.Models.CommonModels.OutputModels;
using ArticleProject.Resources;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;

namespace ArticleProject.Api.Application.Commands.AuthenticationCommands
{
    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, MessageOutputModel>
    {
        private readonly UserManager<User> _userManager;
        private readonly IStringLocalizer<AuthenticationRes> _stringLocalizer;

        public ChangePasswordCommandHandler(UserManager<User> userManager, IStringLocalizer<AuthenticationRes> stringLocalizer)
        {
            _userManager = userManager;
            _stringLocalizer = stringLocalizer;
        }

        public async Task<MessageOutputModel> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var response = new MessageOutputModel();

            var user = await _userManager.FindByEmailAsync(request.Model.Email!);

            if (user == null)
            {
                response.IsSuccessed = false;
                response.Messages.Add(_stringLocalizer["ErrorUnknown"]);
                return response;
            }

            var result = await _userManager.ChangePasswordAsync(user, request.Model.Password!, request.Model.NewPassword!);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Where(p => p.Code != nameof(IdentityErrorDescriber.PasswordMismatch)).Select(p => p.Description).ToList();

                if (!errors.Any())
                {
                    response.Messages.Add(_stringLocalizer["ErrorUnknown"]);
                }
                else
                {
                    response.Messages.AddRange(errors);
                }

                response.IsSuccessed = false;
                return response;
            }

            response.IsSuccessed = true;
            response.Messages.Add(_stringLocalizer["PasswordChangedSuccessfully"]);
            return response;
        }
    }
}