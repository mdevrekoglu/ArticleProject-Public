using ArticleProject.Api.Application.Services;
using ArticleProject.Domain.Aggregates;
using ArticleProject.Domain.Common.Interfaces.Repository;
using ArticleProject.Model.Enums;
using ArticleProject.Model.Models.CommonModels.OutputModels;
using ArticleProject.Resources;
using MediatR;
using Microsoft.Extensions.Localization;

namespace ArticleProject.Api.Application.Commands.AzureFileCommands
{
    public class CreateAzureFileCommandHandler : IRequestHandler<CreateAzureFileCommand, MessageOutputModel>
    {
        private readonly IRepository<AzureFile> _azureFileRepository;
        private readonly AzureBlobService _azureBlobService;
        private readonly CurrentUserCredentialService _currentUserCredentialService;
        private readonly IStringLocalizer<AzureFileRes> _stringLocalizer;

        public CreateAzureFileCommandHandler(IRepository<AzureFile> azureFileRepository, AzureBlobService azureBlobService,
                                            CurrentUserCredentialService currentUserCredentialService, IStringLocalizer<AzureFileRes> stringLocalizer)
        {
            _azureFileRepository = azureFileRepository;
            _azureBlobService = azureBlobService;
            _currentUserCredentialService = currentUserCredentialService;
            _stringLocalizer = stringLocalizer;
        }

        public async Task<MessageOutputModel> Handle(CreateAzureFileCommand request, CancellationToken cancellationToken)
        {
            var response = new MessageOutputModel();

            var guid = Guid.NewGuid().ToString();
            var newAzureFile = new AzureFile(guid, Path.GetFileNameWithoutExtension(request.Model.File.FileName), GetFileType(request.Model.File), ReportTypeEnum.External, _stringLocalizer["ExternalFileAddedByUser"], _currentUserCredentialService.User.Id);

            await _azureFileRepository.AddAndSaveAsync(newAzureFile);

            await _azureBlobService.UploadFileAsync(guid, request.Model.File.OpenReadStream());

            response.IsSuccessed = true;
            response.Messages.Add(_stringLocalizer["FileUploadedSuccessfully"]);
            response.Messages.Add(string.Format(_stringLocalizer["CreatedFileGUID"], guid));
            return response;
        }

        private static FileTypeEnum GetFileType(IFormFile file)
        {
            string extension = Path.GetExtension(file.FileName).ToLower();

            return extension switch
            {
                ".pdf" => FileTypeEnum.PDF,
                ".doc" => FileTypeEnum.Word,
                ".docx" => FileTypeEnum.Word,
                ".xls" => FileTypeEnum.Excel,
                ".xlsx" => FileTypeEnum.Excel,
                ".ppt" => FileTypeEnum.PowerPoint,
                ".pptx" => FileTypeEnum.PowerPoint,
                ".jpg" => FileTypeEnum.Image,
                ".jpeg" => FileTypeEnum.Image,
                ".png" => FileTypeEnum.Image,
                ".gif" => FileTypeEnum.Image,
                ".bmp" => FileTypeEnum.Image,
                ".txt" => FileTypeEnum.Text,
                ".zip" => FileTypeEnum.Zip,
                ".csv" => FileTypeEnum.CSV,
                ".json" => FileTypeEnum.JSON,
                ".xml" => FileTypeEnum.XML,
                _ => throw new ArgumentException("Unsupported file type")
            };
        }
    }
}