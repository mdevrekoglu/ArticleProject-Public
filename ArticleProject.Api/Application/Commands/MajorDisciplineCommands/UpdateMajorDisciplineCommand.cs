using ArticleProject.Model.Models.CommonModels.OutputModels;
using ArticleProject.Model.Models.MajorDisciplineModels;
using MediatR;

namespace ArticleProject.Api.Application.Commands.MajorDisciplineCommands
{
    public class UpdateMajorDisciplineCommand : IRequest<KeyOutputModel>
    {
        public MajorDisciplineUpdateInputModel Model { get; set; }

        public UpdateMajorDisciplineCommand(MajorDisciplineUpdateInputModel request)
        {
            Model = request;
        }
    }
}