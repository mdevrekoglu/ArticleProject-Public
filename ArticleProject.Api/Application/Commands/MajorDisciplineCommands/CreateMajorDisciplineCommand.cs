using ArticleProject.Model.Models.MajorDisciplineModels;
using MediatR;

namespace ArticleProject.Api.Application.Commands.MajorDisciplineCommands
{
    public class CreateMajorDisciplineCommand : IRequest<MajorDisciplineDataOutputModel>
    {
        public MajorDisciplineCreateInputModel Model { get; set; }

        public CreateMajorDisciplineCommand(MajorDisciplineCreateInputModel request)
        {
            Model = request;
        }
    }
}