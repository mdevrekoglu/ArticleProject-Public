using ArticleProject.Domain.Aggregates;
using ArticleProject.Domain.Common.Exceptions;
using ArticleProject.Domain.Common.Interfaces.Repository;
using ArticleProject.Model.Models.CommonModels.OutputModels;
using ArticleProject.Resources;
using MediatR;
using Microsoft.Extensions.Localization;

namespace ArticleProject.Api.Application.Commands.DepartmentCommands
{
    public class UpdateDepartmentCommandHandler : IRequestHandler<UpdateDepartmentCommand, KeyOutputModel>
    {
        private readonly IRepository<Department> _departmentRepository;
        private readonly IStringLocalizer<DepartmentRes> _stringLocalizer;

        public UpdateDepartmentCommandHandler(IRepository<Department> departmentRepository, IStringLocalizer<DepartmentRes> stringLocalizer)
        {
            _departmentRepository = departmentRepository;
            _stringLocalizer = stringLocalizer;
        }

        public async Task<KeyOutputModel> Handle(UpdateDepartmentCommand request, CancellationToken cancellationToken)
        {
            var response = new KeyOutputModel();

            // Check uniqueness rule
            var existingEntityCount = _departmentRepository.GetCountByFilter(p => p.Id != request.Model.Id && p.FacultyId == request.Model.FacultyId && p.DepartmentName == request.Model.DepartmentName);

            if (existingEntityCount > 0)
            {
                response.IsSuccessed = false;
                response.Messages.Add(_stringLocalizer["ErrorDepartmentNameMustBeUniqueToUpdate"]);
                return response;
            }

            var entity = (await _departmentRepository.GetByIdAsync(request.Model.Id)).ThrowIfNull(_stringLocalizer["ErrorNoDepartmentRecordFound"])!;

            entity.Update(request.Model.DepartmentName, request.Model.StudentCount, request.Model.ResearcherCount, request.Model.FacultyId);

            await _departmentRepository.UpdateAndSaveAsync(entity);

            response.Id = entity.Id;
            response.IsSuccessed = true;
            response.Messages.Add(_stringLocalizer["DepartmentUpdatedSuccessfully"]);
            return response;
        }
    }
}