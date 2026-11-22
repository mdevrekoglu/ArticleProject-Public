using ArticleProject.Domain.Aggregates;
using ArticleProject.Domain.Common.Interfaces.Repository;
using ArticleProject.Model.Models.ResearcherModels;
using ArticleProject.Resources;
using MediatR;
using Microsoft.Extensions.Localization;

namespace ArticleProject.Api.Application.Commands.ResearcherCommands
{
    public class UpdateResearcherCommandHandler : IRequestHandler<UpdateResearcherCommand, ResearcherDataOutputModel>
    {
        private readonly IRepository<Researcher> _researcherRepository;
        private readonly IStringLocalizer<ResearcherRes> _stringLocalizer;

        public UpdateResearcherCommandHandler(IRepository<Researcher> researcherRepository, IStringLocalizer<ResearcherRes> stringLocalizer)
        {
            _researcherRepository = researcherRepository;
            _stringLocalizer = stringLocalizer;
        }

        public Task<ResearcherDataOutputModel> Handle(UpdateResearcherCommand request, CancellationToken cancellationToken)
        {
            // TODO MEHMET
            throw new NotImplementedException();
        }
    }
}