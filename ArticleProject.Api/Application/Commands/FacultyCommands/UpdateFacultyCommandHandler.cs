using ArticleProject.Domain.Aggregates;
using ArticleProject.Domain.Common.Exceptions;
using ArticleProject.Domain.Common.Interfaces.Repository;
using ArticleProject.Model.Models.CommonModels.OutputModels;
using ArticleProject.Resources;
using MediatR;
using Microsoft.Extensions.Localization;

namespace ArticleProject.Api.Application.Commands.FacultyCommands
{
    public class UpdateFacultyCommandHandler : IRequestHandler<UpdateFacultyCommand, KeyOutputModel>
    {
        private readonly IRepository<Faculty> _facultyRepository;
        private readonly IStringLocalizer<FacultyRes> _stringLocalizer;

        public UpdateFacultyCommandHandler(IRepository<Faculty> facultyRepository, IStringLocalizer<FacultyRes> stringLocalizer)
        {
            _facultyRepository = facultyRepository;
            _stringLocalizer = stringLocalizer;
        }

        public async Task<KeyOutputModel> Handle(UpdateFacultyCommand request, CancellationToken cancellationToken)
        {
            var response = new KeyOutputModel();

            // Check uniqueness rule
            var existingEntityCount = _facultyRepository.GetCountByFilter(p => p.Id != request.Model.Id && p.AffiliationId == request.Model.AffiliationId && p.FacultyName == request.Model.FacultyName);

            if (existingEntityCount > 0)
            {
                response.IsSuccessed = false;
                response.Messages.Add(_stringLocalizer["ErrorFacultyNameMustBeUniqueToUpdate"]);
                return response;
            }

            var entity = (await _facultyRepository.GetByIdAsync(request.Model.Id)).ThrowIfNull(_stringLocalizer["ErrorNoFacultyRecordFound"])!;

            entity.Update(request.Model.FacultyName, request.Model.AffiliationId);

            await _facultyRepository.UpdateAndSaveAsync(entity);

            response.Id = entity.Id;
            response.IsSuccessed = true;
            response.Messages.Add(_stringLocalizer["FacultyUpdatedSuccessfully"]);
            return response;
        }
    }
}