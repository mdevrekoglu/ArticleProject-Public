using ArticleProject.Api.Application.Services;
using ArticleProject.Domain.Aggregates;
using ArticleProject.Domain.Common.Exceptions;
using ArticleProject.Domain.Common.Extensions;
using ArticleProject.Domain.Common.Interfaces.Query;
using ArticleProject.Domain.Common.Interfaces.Repository;
using ArticleProject.Model.Enums;
using ArticleProject.Model.Models.AzureFileModels;
using ArticleProject.Model.Models.CommonModels.InputModels;
using ArticleProject.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace ArticleProject.Api.Application.Queries
{
    public class AzureFileQuery : IQuery
    {
        private readonly IReadOnlyRepository<AzureFile> _azureFileRepository;
        private readonly IStringLocalizer<AzureFileRes> _stringLocalizer;
        private readonly AzureBlobService _azureBlobService;

        public AzureFileQuery(IReadOnlyRepository<AzureFile> azureFileRepository, IStringLocalizer<AzureFileRes> stringLocalizer, AzureBlobService azureBlobService)
        {
            _azureFileRepository = azureFileRepository;
            _stringLocalizer = stringLocalizer;
            _azureBlobService = azureBlobService;
        }

        public AzureFileListOutputModel GetAllPagedBySearchModel(AzureFileSearchInputModel request)
        {
            var response = new AzureFileListOutputModel();

            var expressionFilter = GetExpressionFilter(request);

            response.Count = _azureFileRepository.GetCountByFilter(expressionFilter);

            if (response.Count == 0)
            {
                response.IsSuccessed = false;
                response.Messages.Add(_stringLocalizer["ErrorNoFileRecordFound"]);
                return response;
            }

            response.Data = _azureFileRepository.GetAllByFilter(expressionFilter, i => i.Include(p => p.User))
                                                    .Select(p => new AzureFileListDataModel
                                                    {
                                                        Id = p.Id,
                                                        FileName = p.FileName,
                                                        FileGUID = p.GUID,
                                                        FileType = p.FileType,
                                                        ReportType = p.ReportType,
                                                        Description = p.Description,
                                                        CreatedDate = p.CreatedDate,

                                                        UserId = p.CreatedBy,
                                                        UserName = string.Concat(p.User.FirstName, " ", p.User.LastName)
                                                    }).OrderBy(request.GetSortingExpression())
                                                    .Skip(request.PageSize * request.PageIndex)
                                                    .Take(request.PageSize)
                                                    .ToList();

            response.IsSuccessed = true;
            response.Messages.Add(_stringLocalizer["FileRecordFoundSuccessfully"]);
            return response;
        }

        private Expression<Func<AzureFile, bool>> GetExpressionFilter(AzureFileSearchInputModel request)
        {
            Expression<Func<AzureFile, bool>> filter = p => true;

            if (!string.IsNullOrWhiteSpace(request.FileName))
            {
                filter = filter.And(p => p.FileName.Contains(request.FileName));
            }

            if (!string.IsNullOrWhiteSpace(request.FileGUID))
            {
                filter = filter.And(p => p.GUID.Contains(request.FileGUID));
            }

            if (!string.IsNullOrWhiteSpace(request.CreatedBy))
            {
                filter = filter.And(p => p.CreatedBy.Equals(request.CreatedBy));
            }

            if (request.FileType.HasValue)
            {
                filter = filter.And(p => p.FileType == request.FileType.Value);
            }

            if (request.ReportType.HasValue)
            {
                filter = filter.And(p => p.ReportType == request.ReportType.Value);
            }

            if (request.StartDate.HasValue)
            {
                filter = filter.And(p => p.CreatedDate.Date >= request.StartDate.Value.Date);
            }

            if (request.EndDate.HasValue)
            {
                filter = filter.And(p => p.CreatedDate.Date <= request.EndDate.Value.Date);
            }

            return filter;
        }

        public async Task<FileResult> DownloadFileByIdAsync(KeyInputModel request)
        {
            var fileEntity = (await _azureFileRepository.GetByIdAsync(request.Id)).ThrowIfNull(_stringLocalizer["ErrorNoFileRecordFound"]);

            var blobStream = await _azureBlobService.DownloadFileAsync(fileEntity.GUID);
            using var memoryStream = new MemoryStream();
            await blobStream.CopyToAsync(memoryStream);

            var content = memoryStream.ToArray();
            var contentType = GetContentType(fileEntity.FileName);

            var extension = GetExtensionFromFileType(fileEntity.FileType);
            var fileNameWithExtension = fileEntity.FileName.EndsWith(extension, StringComparison.OrdinalIgnoreCase)
                ? fileEntity.FileName
                : fileEntity.FileName + extension;

            return new FileContentResult(content, contentType)
            {
                FileDownloadName = fileNameWithExtension
            };
        }

        private string GetExtensionFromFileType(FileTypeEnum fileType)
        {
            return fileType switch
            {
                FileTypeEnum.PDF => ".pdf",
                FileTypeEnum.Word => ".docx",
                FileTypeEnum.Excel => ".xlsx",
                FileTypeEnum.PowerPoint => ".pptx",
                FileTypeEnum.Image => ".png",
                FileTypeEnum.Text => ".txt",
                FileTypeEnum.Zip => ".zip",
                FileTypeEnum.CSV => ".csv",
                FileTypeEnum.JSON => ".json",
                FileTypeEnum.XML => ".xml",
                _ => ""
            };
        }

        private string GetContentType(string fileName)
        {
            var provider = new Microsoft.AspNetCore.StaticFiles.FileExtensionContentTypeProvider();
            return provider.TryGetContentType(fileName, out var contentType)
                ? contentType
                : "application/octet-stream";
        }
    }
}