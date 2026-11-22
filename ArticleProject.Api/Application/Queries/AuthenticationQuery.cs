using ArticleProject.Api.Application.Services;
using ArticleProject.Domain.Aggregates;
using ArticleProject.Domain.Common.Interfaces.Query;
using ArticleProject.Model.Constants;
using ArticleProject.Model.Models.CommonModels.OutputModels;
using ArticleProject.Model.Models.UserModels;
using ArticleProject.Resources;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System.Net;

namespace ArticleProject.Api.Application.Queries
{
    public class AuthenticationQuery : IQuery
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly JwtHandlerService _jwtHandler;
        private readonly SmtpEmailService _emailService;
        private readonly CurrentUserCredentialService _currentUserCredentialService;
        private readonly IStringLocalizer<AuthenticationRes> _stringLocalizer;

        public AuthenticationQuery(UserManager<User> userManager, SignInManager<User> signInManager, JwtHandlerService jwtHandler, SmtpEmailService emailService,
                                    CurrentUserCredentialService currentUserCredentialService, IStringLocalizer<AuthenticationRes> stringLocalizer)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtHandler = jwtHandler;
            _emailService = emailService;
            _currentUserCredentialService = currentUserCredentialService;
            _stringLocalizer = stringLocalizer;
        }

        public async Task<UserLogInResponseModel> LogIn(UserLogInModel request)
        {
            var response = new UserLogInResponseModel();

            var user = await _userManager.FindByEmailAsync(EmailDomainConst.GetFullEmail(request.Email!, request.UserType));
            if (user == null)
            {
                response.IsSuccessed = false;
                response.Messages.Add(_stringLocalizer["ErrorLogIn"]);
                return response;
            }

            var result = await _signInManager.PasswordSignInAsync(user, request.Password!, false, true);
            if (!result.Succeeded)
            {
                response.IsSuccessed = false;

                if (result.IsNotAllowed && !user.EmailConfirmed)
                    response.Messages.Add(_stringLocalizer["ErrorEmailNotConfirmedLogIn"]);
                else if (result.IsLockedOut)
                    response.Messages.Add(_stringLocalizer["ErrorAccountLockedOut"]);
                else // New errors may be added??
                    response.Messages.Add(_stringLocalizer["ErrorUnknownLogIn"]);

                return response;
            }

            var roles = await _userManager.GetRolesAsync(user);
            var tokens = _jwtHandler.CreateToken(user, roles, request.RememberMe);

            if (request.RememberMe)
            {
                user.RefreshToken = tokens.RefreshToken;
                user.RefreshTokenExpiryDate = DateTime.UtcNow.AddDays(3); // Üç gün boyunca geçerliliği var
                await _userManager.UpdateAsync(user);
            }
            else
            {
                user.RefreshToken = null;
                user.RefreshTokenExpiryDate = null;
                await _userManager.UpdateAsync(user);
            }

            // Return a success result with a token and refresh token
            response.Token = tokens.Token;
            response.RefreshToken = tokens.RefreshToken;

            response.IsSuccessed = true;
            response.Messages.Add(_stringLocalizer["UserLoggedInSuccessfully"]);
            return response;
        }

        public async Task<UserTokenOutputModel> RefreshToken(UserTokenInputModel request)
        {
            var response = new UserTokenOutputModel();

            var claimsPrincipal = _jwtHandler.GetPrincipalsFromExpiredToken(request.Token);

            var user = await _userManager.GetUserAsync(claimsPrincipal);

            if (user == null)
            {
                response.IsSuccessed = false;
                response.Messages.Add(_stringLocalizer["ErrorUnknown"]);
                return response;
            }

            var refreshTokenValid = _jwtHandler.ValidateRefreshToken(request.RefreshToken, user.RefreshToken, user.RefreshTokenExpiryDate);

            if (!refreshTokenValid)
            {
                response.IsSuccessed = false;
                response.Messages.Add(_stringLocalizer["ErrorUnknown"]);
                return response;
            }

            var roles = await _userManager.GetRolesAsync(user);
            var tokens = _jwtHandler.CreateToken(user, roles, true);

            user.RefreshToken = tokens.RefreshToken;
            user.RefreshTokenExpiryDate = DateTime.UtcNow.AddDays(3); // Üç gün boyunca geçerliliği var
            await _userManager.UpdateAsync(user);

            // Return a success result with a token and refresh token
            response.IsSuccessed = true;
            response.Messages.Add(_stringLocalizer["TokenRefreshedSuccessfully"]);
            response.Token = tokens.Token;
            response.RefreshToken = tokens.RefreshToken;
            return response;
        }

        public async Task<MessageOutputModel> ResendEmailConfirm(UserRequestModel request)
        {
            var response = new MessageOutputModel();

            var fullEmail = EmailDomainConst.GetFullEmail(request.Email!, request.UserType);
            var user = await _userManager.FindByEmailAsync(fullEmail);

            if (user == null || user.EmailConfirmed)
            {
                response.IsSuccessed = false;
                response.Messages.Add(_stringLocalizer["ErrorUnknown"]);
                return response;
            }

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var query = $"token={WebUtility.UrlEncode(token)}&email={fullEmail}";

            await _emailService.SendEmailAsync(user.Email!, _stringLocalizer["EmailConfirmationSubject"],
                                                _emailService.GenerateCallbackUri(EmailDomainConst.getEmailConfirmPath(), query));

            response.IsSuccessed = true;
            response.Messages.Add(_stringLocalizer["EmailConfirmMailResendSuccessfully"]);
            return response;
        }

        public async Task<MessageOutputModel> ResetPasswordEmail(UserRequestModel request)
        {
            var response = new MessageOutputModel();

            var fullMail = EmailDomainConst.GetFullEmail(request.Email!, request.UserType);
            var user = await _userManager.FindByEmailAsync(fullMail);

            if (user == null)
            {
                response.IsSuccessed = false;
                response.Messages.Add(_stringLocalizer["ErrorUnknown"]);
                return response;
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var query = $"token={WebUtility.UrlEncode(token)}&email={fullMail}";

            await _emailService.SendEmailAsync(user.Email!, _stringLocalizer["PasswordResetSubject"],
                                                _emailService.GenerateCallbackUri(EmailDomainConst.getPasswordResetPath(), query));

            response.IsSuccessed = true;
            response.Messages.Add(_stringLocalizer["PaswordResetEmailSendSuccessfully"]);
            return response;
        }

        public async Task<UserCredentialsModel> GetUserCredentialsAsync()
        {
            var userId = _currentUserCredentialService.User.Id;
            var user = await _userManager.Users.Where(p => p.Id == userId)
                                            .Include(p => p.Department).ThenInclude(p => p.Faculty).ThenInclude(p => p.Affiliation)
                                            .Include(p => p.Image)
                                            .AsNoTracking()
                                            .FirstOrDefaultAsync();

            var response = new UserCredentialsModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Image = user.ImageId != null ? Convert.ToBase64String(user.Image.Image) : null,
                DepartmentName = user.Department?.DepartmentName,
                FacultyName = user.Department?.Faculty.FacultyName,
                AffiliationName = user.Department?.Faculty.Affiliation.AffiliationName
            };

            response.IsSuccessed = true;
            response.Messages.Add(_stringLocalizer["UserCredentialsFoundSuccessfully"]);
            return response;
        }
    }
}