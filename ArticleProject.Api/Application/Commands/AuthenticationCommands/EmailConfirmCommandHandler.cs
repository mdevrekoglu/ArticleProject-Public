using ArticleProject.Domain.Aggregates;
using ArticleProject.Model.Models.CommonModels.OutputModels;
using ArticleProject.Resources;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using System.Net;

namespace ArticleProject.Api.Application.Commands.AuthenticationCommands
{
    public class EmailConfirmCommandHandler : IRequestHandler<EmailConfirmCommand, MessageOutputModel>
    {
        private readonly UserManager<User> _userManager;
        private readonly IStringLocalizer<AuthenticationRes> _stringLocalizer;

        public EmailConfirmCommandHandler(UserManager<User> userManager, IStringLocalizer<AuthenticationRes> stringLocalizer)
        {
            _userManager = userManager;
            _stringLocalizer = stringLocalizer;
        }

        public async Task<MessageOutputModel> Handle(EmailConfirmCommand request, CancellationToken cancellationToken)
        {
            var response = new MessageOutputModel();

            var user = await _userManager.FindByEmailAsync(request.Model.Email!);

            if (user == null || user.EmailConfirmed)
            {
                response.IsSuccessed = false;
                response.Messages.Add(_stringLocalizer["ErrorUnknownEmailConfirm"]);
                return response;
            }

            var token = WebUtility.HtmlDecode(request.Model.Token);
            var result = await _userManager.ConfirmEmailAsync(user, token!);

            if (!result.Succeeded)
            {
                response.IsSuccessed = false;
                var errors = result.Errors.Select(e => e.Description).ToList();
                response.Messages.AddRange(errors);
                return response;
            }

            response.IsSuccessed = true;
            response.Messages.Add(_stringLocalizer["EmailConfirmedSuccessfully"]);
            return response;
        }
    }
}