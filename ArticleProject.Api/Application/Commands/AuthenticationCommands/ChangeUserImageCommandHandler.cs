using ArticleProject.Api.Application.Services;
using ArticleProject.Domain.Aggregates;
using ArticleProject.Domain.Common.Interfaces.Repository;
using ArticleProject.Model.Models.CommonModels.OutputModels;
using ArticleProject.Resources;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace ArticleProject.Api.Application.Commands.AuthenticationCommands
{
    public class ChangeUserImageCommandHandler : IRequestHandler<ChangeUserImageCommand, MessageOutputModel>
    {
        private readonly UserManager<User> _userManager;
        private readonly IRepository<UserImage> _userImageRepository;
        private readonly CurrentUserCredentialService _currentUserCredentialService;
        private readonly IStringLocalizer<AuthenticationRes> _stringLocalizer;

        public ChangeUserImageCommandHandler(UserManager<User> userManager, CurrentUserCredentialService currentUserCredentialService, IRepository<UserImage> userImageRepository,
                                            IStringLocalizer<AuthenticationRes> stringLocalizer)
        {
            _userManager = userManager;
            _userImageRepository = userImageRepository;
            _currentUserCredentialService = currentUserCredentialService;
            _stringLocalizer = stringLocalizer;
        }

        public async Task<MessageOutputModel> Handle(ChangeUserImageCommand request, CancellationToken cancellationToken)
        {
            var response = new MessageOutputModel();

            var userId = _currentUserCredentialService.User.Id;
            var user = await _userManager.Users.Include(p => p.Image).FirstOrDefaultAsync(p => p.Id == userId);

            if (user.ImageId != null)
            {
                await _userImageRepository.DeleteAndSaveAsync(user.Image);
            }

            user.UpdateImage(new UserImage(Convert.FromBase64String(request.Model.Image)));
            await _userManager.UpdateAsync(user);

            response.IsSuccessed = true;
            response.Messages.Add(_stringLocalizer["UserImageChangedSuccessfully"]);
            return response;
        }
    }
}