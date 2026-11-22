using ArticleProject.Domain.Aggregates;
using ArticleProject.Domain.Common.Interfaces.Repository;
using ArticleProject.Model.Models.FacultyModels;
using ArticleProject.Resources;
using MediatR;
using Microsoft.Extensions.Localization;

namespace ArticleProject.Api.Application.Commands.FacultyCommands
{
    public class CreateFacultyCommandHandler : IRequestHandler<CreateFacultyCommand, FacultyDataOutputModel>
    {
        private readonly IRepository<Faculty> _facultyRepository;
        private readonly IStringLocalizer<FacultyRes> _stringLocalizer;

        public CreateFacultyCommandHandler(IRepository<Faculty> facultyRepository, IStringLocalizer<FacultyRes> stringLocalizer)
        {
            _facultyRepository = facultyRepository;
            _stringLocalizer = stringLocalizer;
        }

        public async Task<FacultyDataOutputModel> Handle(CreateFacultyCommand request, CancellationToken cancellationToken)
        {
            var response = new FacultyDataOutputModel();

            // Check uniqueness rule
            var existingEntityCount = _facultyRepository.GetCountByFilter(p => p.AffiliationId == request.Model.AffiliationId && p.FacultyName == request.Model.FacultyName);

            if (existingEntityCount > 0)
            {
                response.IsSuccessed = false;
                response.Messages.Add(_stringLocalizer["ErrorFacultyNameMustBeUniqueToCreate"]);
                return response;
            }

            var entity = new Faculty
            {
                FacultyName = request.Model.FacultyName,
                AffiliationId = request.Model.AffiliationId,
            };

            await _facultyRepository.AddAndSaveAsync(entity, i => i.Affiliation);

            response = new FacultyDataOutputModel
            {
                Id = entity.Id,
                FacultyName = entity.FacultyName,
                AffiliationId = entity.AffiliationId,
                AffiliationName = entity.Affiliation.AffiliationName
            };

            response.IsSuccessed = true;
            response.Messages.Add(_stringLocalizer["FacultyCreatedSuccessfully"]);

            return response;
        }
    }
}