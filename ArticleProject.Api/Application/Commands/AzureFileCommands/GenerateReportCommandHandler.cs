using ArticleProject.Api.Application.Services;
using ArticleProject.Domain.Aggregates;
using ArticleProject.Domain.Common.Interfaces.Repository;
using ArticleProject.Model.Enums;
using ArticleProject.Model.Models.AzureFileModels.GenerateReportModels;
using ArticleProject.Model.Models.CommonModels.OutputModels;
using ArticleProject.Resources;
using ClosedXML.Excel;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace ArticleProject.Api.Application.Commands.AzureFileCommands
{
    public class GenerateReportCommandHandler : IRequestHandler<GenerateReportCommand, MessageOutputModel>
    {
        private readonly IRepository<AzureFile> _azureFileRepository;
        private readonly IReadOnlyRepository<Journal> _journalRepository;
        private readonly IReadOnlyRepository<Researcher> _researcherRepository;
        private readonly AzureBlobService _azureBlobService;
        private readonly CurrentUserCredentialService _currentUserCredentialService;
        private readonly IStringLocalizer<AzureFileRes> _stringLocalizer;

        public GenerateReportCommandHandler(IRepository<AzureFile> azureFileRepository, IReadOnlyRepository<Journal> journalRepository, IReadOnlyRepository<Researcher> researcherRepository,
                                            AzureBlobService azureBlobService, CurrentUserCredentialService currentUserCredentialService,
                                            IStringLocalizer<AzureFileRes> stringLocalizer)
        {
            _azureFileRepository = azureFileRepository;
            _journalRepository = journalRepository;
            _researcherRepository = researcherRepository;
            _azureBlobService = azureBlobService;
            _currentUserCredentialService = currentUserCredentialService;
            _stringLocalizer = stringLocalizer;
        }

        public async Task<MessageOutputModel> Handle(GenerateReportCommand request, CancellationToken cancellationToken)
        {
            var response = new MessageOutputModel();

            // :D?
            QuestPDF.Settings.License = LicenseType.Community;

            var fileExtension = Path.GetExtension(request.Model.File.FileName);

            if (fileExtension != ".xlsx")
                throw new ValidationException(_stringLocalizer["ErrorFileTypeMustBeXLSX"]);

            using var stream = new MemoryStream();
            await request.Model.File.CopyToAsync(stream);
            stream.Position = 0;

            using var workbook = new XLWorkbook(stream);
            var worksheet = workbook.Worksheets.First();

            var expectedHeaders = Enum.GetNames(typeof(GenerateReportExcelHeadersEnum));
            var actualHeaders = worksheet.Row(1)
                .Cells(1, expectedHeaders.Length)
                .Select(c => c.GetString().Trim())
                .ToArray();

            // Check if the headers are valid
            if (!expectedHeaders.SequenceEqual(actualHeaders))
                throw new ValidationException(string.Format(_stringLocalizer["ErrorHeadersAreNotValid"], string.Join(", ", expectedHeaders)));

            // PublicationType Authors AuthorFullNames ArticleTitle SourceTitle
            // DocumentType Affiliations WosIds ORCIDs CitedReferenceCount TimesCitedWoSCore
            // TimesCitedAllDatabases Publisher ISSN eISSN PublicationDate PublicationYear
            // DOI WoSCategories ResearchAreas UniqueWOSID
            var excelData = new List<GenerateReportExcelDataModel>();
            foreach (var row in worksheet.RowsUsed().Skip(1))
            {
                var entity = new GenerateReportExcelDataModel
                {
                    RowNumber = row.RowNumber(),

                    PublicationType = row.Cell(1).GetString().Trim(),
                    Authors = row.Cell(2).GetString().Trim(),
                    AuthorFullNames = row.Cell(3).GetString().Trim(),
                    ArticleTitle = row.Cell(4).GetString().Trim(),
                    SourceTitle = row.Cell(5).GetString().Trim(),
                    DocumentType = row.Cell(6).GetString().Trim(),
                    Affiliations = row.Cell(7).GetString().Trim(),
                    WosIds = row.Cell(8).GetString().Trim(),
                    OrcIDs = row.Cell(9).GetString().Trim(),
                    CitedReferenceCount = decimal.TryParse(row.Cell(10).GetString().Trim(), out var resultCRC) ? resultCRC : (decimal?)null,
                    TimesCitedWoSCore = decimal.TryParse(row.Cell(11).GetString().Trim(), out var resultTCWC) ? resultTCWC : (decimal?)null,
                    TimesCitedAllDatabases = decimal.TryParse(row.Cell(12).GetString().Trim(), out var resultTCAD) ? resultTCAD : (decimal?)null,
                    Publisher = row.Cell(13).GetString().Trim(),
                    ISSN = row.Cell(14).GetString().Trim(),
                    EISSN = row.Cell(15).GetString().Trim(),
                    PublicationDate = row.Cell(16).GetString().Trim(),
                    PublicationYear = decimal.TryParse(row.Cell(17).GetString().Trim(), out var resultPY) ? resultPY : (decimal?)null,
                    DOI = row.Cell(18).GetString().Trim(),
                    WosCategories = row.Cell(19).GetString().Trim(),
                    ResearchAreas = row.Cell(20).GetString().Trim(),
                    UniqueWOSID = row.Cell(21).GetString().Trim()
                };

                if (IsEntityValid(entity))
                    excelData.Add(entity);
            }

            var journalISSNs = excelData.Where(p => !string.IsNullOrWhiteSpace(p.ISSN)).Select(p => p.ISSN).Distinct();
            var journalEISSNs = excelData.Where(p => !string.IsNullOrWhiteSpace(p.EISSN)).Select(p => p.EISSN).Distinct();
            var journals = _journalRepository.GetAllByFilter(p => journalISSNs.Contains(p.ISSN) || journalEISSNs.Contains(p.EISSN), i => i.Include(p => p.Category)).ToList();

            var researcherWosIds = excelData.Where(p => !string.IsNullOrWhiteSpace(p.WosIds))
                                            .SelectMany(p => p.WosIds.Split(';'))
                                            .Select(p => p.Trim())
                                            .Where(p => p.Contains("/"))
                                            .Select(p => p.Split('/').Last())
                                            .Distinct()
                                            .ToList();
            var researchers = _researcherRepository.GetAllByFilter(p => researcherWosIds.Contains(p.WoSId)).ToList();

            switch (request.Model.ReportType)
            {
                case ReportTypeEnum.AffiliationBased:
                    break;

                case ReportTypeEnum.FacultyBased:
                    break;

                case ReportTypeEnum.DepartmentBased:
                    break;

                case ReportTypeEnum.MajorDisciplineBased:
                    break;

                case ReportTypeEnum.ResearcherBased:

                    var enrichedExcelData = excelData.Select(e =>
                    {
                        var matchedJournal = journals.FirstOrDefault(j =>
                            (!string.IsNullOrWhiteSpace(e.ISSN) && j.ISSN == e.ISSN) ||
                            (!string.IsNullOrWhiteSpace(e.EISSN) && j.EISSN == e.EISSN));

                        var quartile = matchedJournal?.Year == e.PublicationYear ? matchedJournal?.Category.Quartile : (int?)null;

                        return new
                        {
                            Excel = e,
                            Quartile = quartile
                        };
                    }).ToList();

                    var reportData = researchers.Select(researcher =>
                    {
                        var relatedArticles = enrichedExcelData.Where(e => e.Excel.WosIds.Contains(researcher.WoSId)).ToList();

                        var publicationsByYear = relatedArticles
                            .Where(e => e.Excel.PublicationYear.HasValue)
                            .GroupBy(e => (int)e.Excel.PublicationYear.Value)
                            .Select(group =>
                            {
                                var q1Count = group.Count(x => x.Quartile == (int)JournalQuartileEnum.Q1);
                                var q2Count = group.Count(x => x.Quartile == (int)JournalQuartileEnum.Q2);
                                var q3Count = group.Count(x => x.Quartile == (int)JournalQuartileEnum.Q3);
                                var q4Count = group.Count(x => x.Quartile == (int)JournalQuartileEnum.Q4);
                                var unknownCount = group.Count(x => x.Quartile == null);

                                return new PublicationsByYear
                                {
                                    Year = group.Key,
                                    Q1Count = q1Count,
                                    Q2Count = q2Count,
                                    Q3Count = q3Count,
                                    Q4Count = q4Count,
                                    NotCategorizedCount = unknownCount
                                };
                            }).OrderByDescending(p => p.Year).ToList();

                        return new ResearcherBasedReportModel
                        {
                            ResearcherId = researcher.Id,
                            FirstName = researcher.FirstName,
                            LastName = researcher.LastName,
                            Email = researcher.Email,
                            Position = researcher.Position,
                            StaffTitle = researcher.StaffTitle,
                            ScopusId = researcher.ScopusId,
                            WoSId = researcher.WoSId,
                            ORCId = researcher.ORCId,
                            Publications = publicationsByYear
                        };
                    }).ToList();

                    switch (request.Model.FileType)
                    {
                        case FileTypeEnum.PDF:

                            var pdfStream = new MemoryStream();

                            // QuestPDF kullanımı
                            Document.Create(container =>
                            {
                                container.Page(page =>
                                {
                                    page.Margin(20);
                                    page.Header().Text("Researcher Report").FontSize(20).Bold().AlignCenter();

                                    page.Content().Table(table =>
                                    {
                                        table.ColumnsDefinition(columns =>
                                        {
                                            columns.RelativeColumn(); // Name
                                            columns.RelativeColumn(); // Email
                                            columns.RelativeColumn(); // Year
                                            columns.RelativeColumn(); // Q1
                                            columns.RelativeColumn(); // Q2
                                            columns.RelativeColumn(); // Q3
                                            columns.RelativeColumn(); // Q4
                                            columns.RelativeColumn(); // Not Categorized
                                        });

                                        // Header
                                        table.Header(header =>
                                        {
                                            header.Cell().Text("Name").Bold();
                                            header.Cell().Text("Email").Bold();
                                            header.Cell().Text("Year").Bold();
                                            header.Cell().Text("Q1").Bold();
                                            header.Cell().Text("Q2").Bold();
                                            header.Cell().Text("Q3").Bold();
                                            header.Cell().Text("Q4").Bold();
                                            header.Cell().Text("N/A").Bold();
                                        });

                                        foreach (var researcher in reportData)
                                        {
                                            foreach (var pub in researcher.Publications)
                                            {
                                                table.Cell().Text($"{researcher.FirstName} {researcher.LastName}");
                                                table.Cell().Text(researcher.Email);
                                                table.Cell().Text(pub.Year.ToString());
                                                table.Cell().Text(pub.Q1Count.ToString());
                                                table.Cell().Text(pub.Q2Count.ToString());
                                                table.Cell().Text(pub.Q3Count.ToString());
                                                table.Cell().Text(pub.Q4Count.ToString());
                                                table.Cell().Text(pub.NotCategorizedCount.ToString());
                                            }
                                        }
                                    });
                                });
                            })
                            .GeneratePdf(pdfStream);

                            pdfStream.Position = 0;

                            var pdfGUID = Guid.NewGuid().ToString();
                            var pdfAzureFile = new AzureFile(pdfGUID, request.Model.FileName, FileTypeEnum.PDF, ReportTypeEnum.ResearcherBased, request.Model.Description, _currentUserCredentialService.User.Id);
                            await _azureFileRepository.AddAndSaveAsync(pdfAzureFile);
                            await _azureBlobService.UploadFileAsync(pdfGUID, pdfStream);

                            response.Messages.Add(_stringLocalizer["ReportCreatedSuccessfully"]);
                            response.Messages.Add(string.Format(_stringLocalizer["CreatedFileGUID"], pdfGUID));
                            response.IsSuccessed = true;

                            break;

                        case FileTypeEnum.Word:
                            break;

                        case FileTypeEnum.Excel:

                            using (var reportWorkbook = new XLWorkbook())
                            {
                                var ws = reportWorkbook.Worksheets.Add("Researcher Report");
                                // Başlıklar
                                ws.Cell(1, 1).Value = "Researcher ID";
                                ws.Cell(1, 2).Value = "Name";
                                ws.Cell(1, 3).Value = "Email";
                                ws.Cell(1, 4).Value = "Position";
                                ws.Cell(1, 5).Value = "WoSId";
                                ws.Cell(1, 6).Value = "ORCId";
                                ws.Cell(1, 7).Value = "ScopusId";
                                ws.Cell(1, 8).Value = "Year";
                                ws.Cell(1, 9).Value = "Q1";
                                ws.Cell(1, 10).Value = "Q2";
                                ws.Cell(1, 11).Value = "Q3";
                                ws.Cell(1, 12).Value = "Q4";
                                ws.Cell(1, 13).Value = "Uncategorized";

                                var rowIndex = 2;
                                foreach (var researcher in reportData)
                                {
                                    foreach (var pub in researcher.Publications)
                                    {
                                        ws.Cell(rowIndex, 1).Value = researcher.ResearcherId;
                                        ws.Cell(rowIndex, 2).Value = $"{researcher.FirstName} {researcher.LastName}";
                                        ws.Cell(rowIndex, 3).Value = researcher.Email;
                                        ws.Cell(rowIndex, 4).Value = researcher.Position;
                                        ws.Cell(rowIndex, 5).Value = researcher.WoSId;
                                        ws.Cell(rowIndex, 6).Value = researcher.ORCId;
                                        ws.Cell(rowIndex, 7).Value = researcher.ScopusId;
                                        ws.Cell(rowIndex, 8).Value = pub.Year;
                                        ws.Cell(rowIndex, 9).Value = pub.Q1Count;
                                        ws.Cell(rowIndex, 10).Value = pub.Q2Count;
                                        ws.Cell(rowIndex, 11).Value = pub.Q3Count;
                                        ws.Cell(rowIndex, 12).Value = pub.Q4Count;
                                        ws.Cell(rowIndex, 13).Value = pub.NotCategorizedCount;

                                        rowIndex++;
                                    }
                                }

                                // Dosyayı bellekte oluştur
                                using var outStream = new MemoryStream();
                                reportWorkbook.SaveAs(outStream);
                                outStream.Position = 0;

                                var excelGUID = Guid.NewGuid().ToString();
                                var azureFile = new AzureFile(excelGUID, request.Model.FileName, FileTypeEnum.Excel, ReportTypeEnum.ResearcherBased, request.Model.Description, _currentUserCredentialService.User.Id);
                                await _azureFileRepository.AddAndSaveAsync(azureFile);

                                await _azureBlobService.UploadFileAsync(excelGUID, outStream);

                                response.Messages.Add(_stringLocalizer["ReportCreatedSuccessfully"]);
                                response.Messages.Add(string.Format(_stringLocalizer["CreatedFileGUID"], excelGUID));
                                response.IsSuccessed = true;
                            }
                            break;

                        case FileTypeEnum.PowerPoint:
                            break;

                        case FileTypeEnum.Image:
                            break;

                        case FileTypeEnum.Text:
                            break;

                        case FileTypeEnum.Zip:
                            break;

                        case FileTypeEnum.CSV:
                            break;

                        case FileTypeEnum.JSON:
                            break;

                        case FileTypeEnum.XML:
                            break;

                        default:
                            break;
                    }

                    break;

                default:
                    break;
            }

            return response;
        }

        private bool IsEntityValid(GenerateReportExcelDataModel entity)
        {
            if (string.IsNullOrEmpty(entity.WosIds))
                return false;

            if (string.IsNullOrEmpty(entity.ISSN) && string.IsNullOrEmpty(entity.EISSN))
                return false;

            if (!entity.PublicationYear.HasValue)
                return false;

            if (string.IsNullOrEmpty(entity.DOI))
                return false;

            return true;
        }
    }
}