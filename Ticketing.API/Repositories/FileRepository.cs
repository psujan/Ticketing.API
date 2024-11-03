using Ticketing.API.Data;
using Ticketing.API.Model.Domain;
using Ticketing.API.Repositories.Interfaces;
using Ticketing.API.Services;
using FileUploadModel = Ticketing.API.Model.UploadFile;
using IFileRepository = Ticketing.API.Repositories.Interfaces.IFileRepository;

namespace Ticketing.API.Repositories
{
    public class FileRepository : IFileRepository
    {
        private readonly IFileUploadService uploadService;
        private readonly TicketingDbContext dbContext;

        public FileRepository(IFileUploadService uploadService , TicketingDbContext dbContext) 
        {
            this.uploadService = uploadService;
            this.dbContext = dbContext;
        }
        public async Task<Model.Domain.File?> UploadFile(IFormFile file , string? model , string ? uploadDir , int ? modelId)
        {
            FileUploadModel? uploadedFile = await uploadService.UploadFile(file , model , uploadDir);
            if(uploadedFile == null)
            {
                return null;
            }

            var fileRow = new Model.Domain.File()
            {
                Name = uploadedFile.FileName,
                OriginalName = uploadedFile.OriginalName ,
                MimeType =  uploadedFile.Extension,
                Path = uploadedFile.Path,
                Size = uploadedFile.ByteSize,
                Model = model,
                ModelId = modelId
            };

            dbContext.File.Add(fileRow);
            dbContext.SaveChanges();
            return fileRow;
        }

       
        public async Task<IEnumerable<Model.Domain.File>> UploadFiles(List<IFormFile> filesToBeUploaded , string? model, string? uploadDir , int? modelId)
        {
            if(filesToBeUploaded == null)
            {
                return Enumerable.Empty<Model.Domain.File>();
            }

            var fileList = new List<Model.Domain.File>();
            foreach (var file in filesToBeUploaded)
            {
                var result = await UploadFile(file , model , uploadDir , modelId);
                if(result != null)
                {
                    fileList.Add(result);
                }
            }

            return fileList;
        }

        public void DeleteFile(string uploadDir , string fileName)
        {
            uploadService.DeleteFileIfExists(uploadDir , fileName);
        }
        
    }
}
