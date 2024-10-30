using Ticketing.API.Model;

namespace Ticketing.API.Services
{
    public interface IFileUploadService
    {
        Task<UploadFile?> UploadFile(IFormFile file, string ? model, string ? uploadDir = null);

        void DeleteFileIfExists(string dirPath , string fileName);
    }
}
