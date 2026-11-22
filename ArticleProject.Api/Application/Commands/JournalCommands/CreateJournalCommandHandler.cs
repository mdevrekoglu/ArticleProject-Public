using ArticleProject.Domain.Aggregates;
using ArticleProject.Domain.Common.Interfaces.Repository;
using ArticleProject.Model.Models.JournalModels;
using ArticleProject.Resources;
using MediatR;
using Microsoft.Extensions.Localization;
using Category = ArticleProject.Domain.Aggregates.Category;

namespace ArticleProject.Api.Application.Commands.JournalCommands
{
    public class CreateJournalCommandHandler : IRequestHandler<CreateJournalCommand, JournalDataOutputModel>
    {
        private readonly IRepository<Journal> _journalRepository;
        private readonly IStringLocalizer<JournalRes> _stringLocalizer;

        public CreateJournalCommandHandler(IRepository<Journal> journalRepository, IStringLocalizer<JournalRes> stringLocalizer)
        {
            _journalRepository = journalRepository;
            _stringLocalizer = stringLocalizer;
        }

        public async Task<JournalDataOutputModel> Handle(CreateJournalCommand request, CancellationToken cancellationToken)
        {
            var response = new JournalDataOutputModel();

            // Check uniqueness rule
            var existingEntityCount = _journalRepository.GetCountByFilter(p => p.Name == request.Model.Name);

            if (existingEntityCount > 0)
            {
                response.IsSuccessed = false;
                response.Messages.Add(_stringLocalizer["ErrorJournalMustBeUniqueToCreate"]);
                return response;
            }

            var category = new Category(request.Model.CategoryName, request.Model.Quartile, request.Model.Rank, request.Model.TotalJournals);
            var entity = new Journal(request.Model.Name, request.Model.AbbreviationName, request.Model.ISSN, request.Model.EISSN, request.Model.JIF, request.Model.JIF5Years, request.Model.Year, category);

            await _journalRepository.AddAndSaveAsync(entity);

            response = new JournalDataOutputModel
            {
                Id = entity.Id,
                Name = entity.Name,
                AbbreviationName = entity.AbbreviationName,
                ISSN = entity.ISSN,
                EISSN = entity.EISSN,
                JIF = entity.JIF,
                JIF5Years = entity.JIF5Years,
                CategoryId = entity.CategoryId,
                CategoryName = entity.Category.Name,
                Year = entity.Year,
            };

            response.IsSuccessed = true;
            response.Messages.Add(_stringLocalizer["JournalCreatedSuccessfully"]);

            return response;
        }
    }
}