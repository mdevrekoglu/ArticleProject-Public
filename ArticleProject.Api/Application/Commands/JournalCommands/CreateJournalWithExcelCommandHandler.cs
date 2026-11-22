using ArticleProject.Domain.Aggregates;
using ArticleProject.Domain.Common.Interfaces.Repository;
using ArticleProject.Model.Enums;
using ArticleProject.Model.Models.CommonModels.OutputModels;
using ArticleProject.Resources;
using ClosedXML.Excel;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Localization;

namespace ArticleProject.Api.Application.Commands.JournalCommands
{
    public class CreateJournalWithExcelCommandHandler : IRequestHandler<CreateJournalWithExcelCommand, MessageOutputModel>
    {
        private readonly IRepository<Journal> _journalRepository;
        private readonly IStringLocalizer<JournalRes> _stringLocalizer;

        public CreateJournalWithExcelCommandHandler(IRepository<Journal> journalRepository, IStringLocalizer<JournalRes> stringLocalizer)
        {
            _journalRepository = journalRepository;
            _stringLocalizer = stringLocalizer;
        }

        public async Task<MessageOutputModel> Handle(CreateJournalWithExcelCommand request, CancellationToken cancellationToken)
        {
            var response = new MessageOutputModel();

            var fileExtension = Path.GetExtension(request.Model.File.FileName);

            // If file is not xlsx, throw validation exception
            if (fileExtension != ".xlsx")
                throw new ValidationException(_stringLocalizer["ErrorFileTypeMustBeXLSX"]);

            using var stream = new MemoryStream();
            await request.Model.File.CopyToAsync(stream);
            stream.Position = 0;

            using var workbook = new XLWorkbook(stream);
            var worksheet = workbook.Worksheets.First();

            var expectedHeaders = Enum.GetNames(typeof(JournalExcelHeadersEnum));
            var actualHeaders = worksheet.Row(1)
                .Cells(1, expectedHeaders.Length)
                .Select(c => c.GetString().Trim())
                .ToArray();

            // Check if the headers are valid
            if (!expectedHeaders.SequenceEqual(actualHeaders))
                throw new ValidationException(string.Format(_stringLocalizer["ErrorHeadersAreNotValid"], string.Join(", ", expectedHeaders)));

            var journalList = new List<Journal>();
            foreach (var row in worksheet.RowsUsed().Skip(1)) // Skip header
            {
                int rowNumber = row.RowNumber();

                var name = row.Cell(1).GetString().Trim();
                var abbreviationName = row.Cell(2).GetString().Trim();
                var issn = !string.IsNullOrEmpty(row.Cell(3).GetString().Trim().Replace("N/A", "")) ? row.Cell(3).GetString().Trim().Replace("N/A", "") : null;
                var eissn = !string.IsNullOrEmpty(row.Cell(4).GetString().Trim().Replace("N/A", "")) ? row.Cell(4).GetString().Trim().Replace("N/A", "") : null;
                var jif = row.Cell(5).GetString().Trim().Replace("N/A", "");
                var jif5Years = row.Cell(6).GetString().Trim().Replace("N/A", "");
                var category = row.Cell(7).GetString().Trim();
                var year = row.Cell(8).GetString().Trim();

                var jifDecimal = decimal.TryParse(jif.Replace("<", ""), out var jifValue) ? jifValue : (decimal?)null;
                var jif5YearsDecimal = decimal.TryParse(jif5Years.Replace("<", ""), out var jif5YearsValue) ? jif5YearsValue : (decimal?)null;
                var yearInt = int.TryParse(year, out var yearValue) ? yearValue : throw new ValidationException(string.Format(_stringLocalizer["ErrorYearValueIsNotInt"], rowNumber));

                var categorySplitted = category.Split("|");

                if (categorySplitted.Length != 3)
                    throw new ValidationException(string.Format(_stringLocalizer["ErrorCategoryFormatIsNotValid"], rowNumber));

                var categoryName = categorySplitted[0];
                var quartile = Enum.TryParse<JournalQuartileEnum>(categorySplitted[1], out var quartileValue) ? (int)quartileValue : (int?)null;

                var rankSplitted = categorySplitted[2].Split('/');
                if (rankSplitted.Length != 2)
                    throw new ValidationException(string.Format(_stringLocalizer["ErrorCategoryRankFormatIsNotValid"], rowNumber));

                var rank = int.TryParse(rankSplitted[0], out var rankValue) ? rankValue : (int?)null;
                var totalJournals = int.TryParse(rankSplitted[1], out var totalJournalsValue) ? totalJournalsValue : (int?)null;

                journalList.Add(new Journal(name, abbreviationName, issn, eissn, jifDecimal, jif5YearsDecimal, yearInt, new Category(categoryName, quartile, rank, totalJournals)));
            }

            await _journalRepository.AddRangeAndSaveAsync(journalList);

            response.Messages.Add(_stringLocalizer["JournalsCreatedSuccessfully"]);
            response.IsSuccessed = true;
            return response;
        }
    }
}