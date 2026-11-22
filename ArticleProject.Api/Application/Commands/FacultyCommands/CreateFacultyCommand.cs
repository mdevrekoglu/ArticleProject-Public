using ArticleProject.Model.Models.FacultyModels;
using MediatR;

namespace ArticleProject.Api.Application.Commands.FacultyCommands
{
    public class CreateFacultyCommand : IRequest<FacultyDataOutputModel>
    {
        public FacultyCreateInputModel Model { get; set; }

        public CreateFacultyCommand(FacultyCreateInputModel request)
        {
            Model = request;
        }
    }
}