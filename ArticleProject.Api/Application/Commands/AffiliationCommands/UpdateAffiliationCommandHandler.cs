using ArticleProject.Domain.Aggregates;
using ArticleProject.Domain.Common.Exceptions;
using ArticleProject.Domain.Common.Interfaces.Repository;
using ArticleProject.Model.Models.CommonModels.OutputModels;
using ArticleProject.Resources;
using MediatR;
using Microsoft.Extensions.Localization;

namespace ArticleProject.Api.Application.Commands.AffiliationCommands
{
    public class UpdateAffiliationCommandHandler : IRequestHandler<UpdateAffiliationCommand, KeyOutputModel>
    {
        private readonly IRepository<Affiliation> _affiliationRepository;
        private readonly IStringLocalizer<AffiliationRes> _stringLocalizer;

        public UpdateAffiliationCommandHandler(IRepository<Affiliation> affiliationRepository, IStringLocalizer<AffiliationRes> stringLocalizer)
        {
            _affiliationRepository = affiliationRepository;
            _stringLocalizer = stringLocalizer;
        }

        public async Task<KeyOutputModel> Handle(UpdateAffiliationCommand request, CancellationToken cancellationToken)
        {
            var response = new KeyOutputModel();

            // Check uniqueness rule
            var existingEntityCount = _affiliationRepository.GetCountByFilter(p => p.Id != request.Model.Id && (p.AffiliationName == request.Model.AffiliationName || p.ScopusId == request.Model.ScopusId));

            if (existingEntityCount > 0)
            {
                response.IsSuccessed = false;
                response.Messages.Add(_stringLocalizer["ErrorAffiliationMustBeUniqueToUpdate"]);
                return response;
            }

            var entity = (await _affiliationRepository.GetByIdAsync(request.Model.Id)).ThrowIfNull(_stringLocalizer["ErrorNoAffiliationRecordFound"])!;

            entity.Update(request.Model.AffiliationName, request.Model.Country, request.Model.City, request.Model.Address, request.Model.ScopusId, request.Model.OrganizationType);

            await _affiliationRepository.UpdateAndSaveAsync(entity);

            response.Id = entity.Id;
            response.IsSuccessed = true;
            response.Messages.Add(_stringLocalizer["AffiliationUpdatedSuccessfully"]);
            return response;
        }
    }
}