using ArticleProject.Domain.Aggregates;
using ArticleProject.Domain.Common.Interfaces.Repository;
using ArticleProject.Model.Models.MajorDisciplineModels;
using ArticleProject.Resources;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace ArticleProject.Api.Application.Commands.MajorDisciplineCommands
{
    public class CreateMajorDisciplineCommandHandler : IRequestHandler<CreateMajorDisciplineCommand, MajorDisciplineDataOutputModel>
    {
        private readonly IRepository<MajorDiscipline> _majorDisciplineRepository;
        private readonly IStringLocalizer<MajorDisciplineRes> _stringLocalizer;

        public CreateMajorDisciplineCommandHandler(IRepository<MajorDiscipline> majorDisciplineRepository, IStringLocalizer<MajorDisciplineRes> stringLocalizer)
        {
            _majorDisciplineRepository = majorDisciplineRepository;
            _stringLocalizer = stringLocalizer;
        }

        public async Task<MajorDisciplineDataOutputModel> Handle(CreateMajorDisciplineCommand request, CancellationToken cancellationToken)
        {
            var response = new MajorDisciplineDataOutputModel();

            // Check uniqueness rule
            var existingEntityCount = _majorDisciplineRepository.GetCountByFilter(p => p.DepartmentId == request.Model.DepartmentId && p.MajorDisciplineName == request.Model.MajorDisciplineName);

            if (existingEntityCount > 0)
            {
                response.IsSuccessed = false;
                response.Messages.Add(_stringLocalizer["ErrorMajorDisciplineNameMustBeUniqueToCreate"]);
                return response;
            }

            var entity = new MajorDiscipline
            {
                MajorDisciplineName = request.Model.MajorDisciplineName,
                DepartmentId = request.Model.DepartmentId,
            };

            await _majorDisciplineRepository.AddAndSaveAsync(entity, i => i.Department);

            entity = await _majorDisciplineRepository.GetByIdAsync(entity.Id, i => i.Include(p => p.Department).ThenInclude(f => f.Faculty).ThenInclude(a => a.Affiliation));

            response = new MajorDisciplineDataOutputModel
            {
                Id = entity.Id,
                MajorDisciplineName = entity.MajorDisciplineName,
                DepartmentId = entity.DepartmentId,
                DepartmentName = entity.Department.DepartmentName,
                FacultyId = entity.Department.FacultyId,
                FacultyName = entity.Department.Faculty.FacultyName,
                AffiliationId = entity.Department.Faculty.Affiliation.Id,
                AffiliationName = entity.Department.Faculty.Affiliation.AffiliationName,
            };

            response.IsSuccessed = true;
            response.Messages.Add(_stringLocalizer["MajorDisciplineCreatedSuccessfully"]);

            return response;
        }
    }
}