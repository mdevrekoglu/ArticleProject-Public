using ArticleProject.Api.Application.Services;
using ArticleProject.Domain.Aggregates;
using ArticleProject.Model.Constants;
using ArticleProject.Model.Enums;
using ArticleProject.Model.Models.CommonModels.OutputModels;
using ArticleProject.Resources;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using System.Net;

namespace ArticleProject.Api.Application.Commands.AuthenticationCommands
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, MessageOutputModel>
    {
        private readonly UserManager<User> _userManager;
        private readonly IStringLocalizer<AuthenticationRes> _stringLocalizer;
        private readonly SmtpEmailService _emailService;

        public RegisterCommandHandler(UserManager<User> userManager, IStringLocalizer<AuthenticationRes> stringLocalizer, SmtpEmailService emailService)
        {
            _userManager = userManager;
            _stringLocalizer = stringLocalizer;
            _emailService = emailService;
        }

        public async Task<MessageOutputModel> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var response = new MessageOutputModel();

            var user = new User
            {
                FirstName = request.Model.FirstName!,
                LastName = request.Model.LastName!,
                Email = EmailDomainConst.GetFullEmail(request.Model.Email!, request.Model.UserType),
                UserName = request.Model.Email
            };

            var result = await _userManager.CreateAsync(user, request.Model.Password!);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                response.Messages.AddRange(errors);
                response.IsSuccessed = false;

                return response;
            }

            switch (request.Model.UserType)
            {
                case (int)UserTypeEnum.Student:
                    await _userManager.AddToRoleAsync(user, "Student");
                    break;

                case (int)UserTypeEnum.Lecturer:
                    await _userManager.AddToRoleAsync(user, "Lecturer");
                    break;
            }

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var query = $"token={WebUtility.UrlEncode(token)}&email={user.Email}";

            await _emailService.SendEmailAsync(user.Email, _stringLocalizer["EmailConfirmationSubject"], _emailService.GenerateCallbackUri(EmailDomainConst.getEmailConfirmPath(), query));

            response.IsSuccessed = true;
            response.Messages.Add(_stringLocalizer["UserRegisteredSuccessfully"]);
            response.Messages.Add(_stringLocalizer["PleaseConfirmEmail"]);
            return response;
        }
    }
}