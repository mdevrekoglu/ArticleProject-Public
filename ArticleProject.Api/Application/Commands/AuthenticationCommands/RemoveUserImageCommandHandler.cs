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
    public class RemoveUserImageCommandHandler : IRequestHandler<RemoveUserImageCommand, MessageOutputModel>
    {
        private readonly UserManager<User> _userManager;
        private readonly IRepository<UserImage> _userImageRepository;
        private readonly CurrentUserCredentialService _currentUserCredentialService;
        private readonly IStringLocalizer<AuthenticationRes> _stringLocalizer;

        public RemoveUserImageCommandHandler(UserManager<User> userManager, IRepository<UserImage> userImageRepository,
                                            CurrentUserCredentialService currentUserCredentialService, IStringLocalizer<AuthenticationRes> stringLocalizer)
        {
            _userManager = userManager;
            _userImageRepository = userImageRepository;
            _currentUserCredentialService = currentUserCredentialService;
            _stringLocalizer = stringLocalizer;
        }

        public async Task<MessageOutputModel> Handle(RemoveUserImageCommand request, CancellationToken cancellationToken)
        {
            var response = new MessageOutputModel();

            var userId = _currentUserCredentialService.User.Id;
            var user = await _userManager.Users.Include(p => p.Image).FirstOrDefaultAsync(p => p.Id == userId);

            if (user.ImageId != null)
            {
                await _userImageRepository.DeleteAndSaveAsync(user.Image);
            }

            response.IsSuccessed = true;
            response.Messages.Add(_stringLocalizer["UserImageRemovedSuccessfully"]);
            return response;
        }
    }
}