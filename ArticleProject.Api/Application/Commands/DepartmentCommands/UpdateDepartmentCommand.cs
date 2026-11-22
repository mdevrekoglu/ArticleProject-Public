using ArticleProject.Model.Models.CommonModels.OutputModels;
using ArticleProject.Model.Models.DepartmentModels;
using MediatR;

namespace ArticleProject.Api.Application.Commands.DepartmentCommands
{
    public class UpdateDepartmentCommand : IRequest<KeyOutputModel>
    {
        public DepartmentUpdateInputModel Model { get; set; }

        public UpdateDepartmentCommand(DepartmentUpdateInputModel request)
        {
            Model = request;
        }
    }
}