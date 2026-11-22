using ArticleProject.Domain.Aggregates;
using ArticleProject.Domain.Common.Exceptions;
using ArticleProject.Domain.Common.Interfaces.Repository;
using ArticleProject.Model.Models.CommonModels.OutputModels;
using ArticleProject.Resources;
using MediatR;
using Microsoft.Extensions.Localization;

namespace ArticleProject.Api.Application.Commands.MajorDisciplineCommands
{
    public class UpdateMajorDisciplineCommandHandler : IRequestHandler<UpdateMajorDisciplineCommand, KeyOutputModel>
    {
        private readonly IRepository<MajorDiscipline> _majorDisciplineRepository;
        private readonly IStringLocalizer<MajorDisciplineRes> _stringLocalizer;

        public UpdateMajorDisciplineCommandHandler(IRepository<MajorDiscipline> majorDisciplineRepository, IStringLocalizer<MajorDisciplineRes> stringLocalizer)
        {
            _majorDisciplineRepository = majorDisciplineRepository;
            _stringLocalizer = stringLocalizer;
        }

        public async Task<KeyOutputModel> Handle(UpdateMajorDisciplineCommand request, CancellationToken cancellationToken)
        {
            var response = new KeyOutputModel();

            // Check uniqueness rule
            var existingEntityCount = _majorDisciplineRepository.GetCountByFilter(p => p.Id != request.Model.Id && p.DepartmentId == request.Model.DepartmentId && p.MajorDisciplineName == request.Model.MajorDisciplineName);

            if (existingEntityCount > 0)
            {
                response.IsSuccessed = false;
                response.Messages.Add(_stringLocalizer["ErrorMajorDisciplineNameMustBeUniqueToUpdate"]);
                return response;
            }

            var entity = (await _majorDisciplineRepository.GetByIdAsync(request.Model.Id)).ThrowIfNull(_stringLocalizer["ErrorNoMajorDisciplineRecordFound"])!;

            entity.Update(request.Model.MajorDisciplineName, request.Model.DepartmentId);

            await _majorDisciplineRepository.UpdateAndSaveAsync(entity);

            response.Id = entity.Id;
            response.IsSuccessed = true;
            response.Messages.Add(_stringLocalizer["MajorDisciplineUpdatedSuccessfully"]);
            return response;
        }
    }
}