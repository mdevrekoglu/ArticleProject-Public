using ArticleProject.Model.Models.CommonModels.OutputModels;
using ArticleProject.Model.Models.FacultyModels;
using MediatR;

namespace ArticleProject.Api.Application.Commands.FacultyCommands
{
    public class UpdateFacultyCommand : IRequest<KeyOutputModel>
    {
        public FacultyUpdateInputModel Model { get; set; }

        public UpdateFacultyCommand(FacultyUpdateInputModel request)
        {
            Model = request;
        }
    }
}