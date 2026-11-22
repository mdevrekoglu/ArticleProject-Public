using ArticleProject.Model.Models.DepartmentModels;
using MediatR;

namespace ArticleProject.Api.Application.Commands.DepartmentCommands
{
    public class CreateDepartmentCommand : IRequest<DepartmentDataOutputModel>
    {
        public DepartmentCreateInputModel Model { get; set; }

        public CreateDepartmentCommand(DepartmentCreateInputModel request)
        {
            Model = request;
        }
    }
}