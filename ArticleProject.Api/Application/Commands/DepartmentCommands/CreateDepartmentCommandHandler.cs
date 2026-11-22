using ArticleProject.Domain.Aggregates;
using ArticleProject.Domain.Common.Interfaces.Repository;
using ArticleProject.Model.Models.DepartmentModels;
using ArticleProject.Resources;
using MediatR;
using Microsoft.Extensions.Localization;

namespace ArticleProject.Api.Application.Commands.DepartmentCommands
{
    public class CreateDepartmentCommandHandler : IRequestHandler<CreateDepartmentCommand, DepartmentDataOutputModel>
    {
        private readonly IRepository<Department> _departmentRepository;
        private readonly IStringLocalizer<DepartmentRes> _stringLocalizer;

        public CreateDepartmentCommandHandler(IRepository<Department> departmentRepository, IStringLocalizer<DepartmentRes> stringLocalizer)
        {
            _departmentRepository = departmentRepository;
            _stringLocalizer = stringLocalizer;
        }

        public async Task<DepartmentDataOutputModel> Handle(CreateDepartmentCommand request, CancellationToken cancellationToken)
        {
            var response = new DepartmentDataOutputModel();

            // Check uniqueness rule
            var existingEntityCount = _departmentRepository.GetCountByFilter(p => p.FacultyId == request.Model.FacultyId && p.DepartmentName == request.Model.DepartmentName);

            if (existingEntityCount > 0)
            {
                response.IsSuccessed = false;
                response.Messages.Add(_stringLocalizer["ErrorDepartmentNameMustBeUniqueToCreate"]);
                return response;
            }

            var entity = new Department
            {
                DepartmentName = request.Model.DepartmentName,
                StudentCount = request.Model.StudentCount,
                ResearcherCount = request.Model.ResearcherCount,
                FacultyId = request.Model.FacultyId,
            };

            await _departmentRepository.AddAndSaveAsync(entity);

            // Webde buradaki bilgilerin yeterli olacağını varsayıyorum olmaz ise tekrar değiştirilmesi gerekiyor.
            response = new DepartmentDataOutputModel
            {
                Id = entity.Id,
                DepartmentName = entity.DepartmentName,
                StudentCount = entity.StudentCount,
                ResearcherCount = entity.ResearcherCount,
                FacultyId = entity.FacultyId
            };

            response.IsSuccessed = true;
            response.Messages.Add(_stringLocalizer["DepartmentCreatedSuccessfully"]);

            return response;
        }
    }
}