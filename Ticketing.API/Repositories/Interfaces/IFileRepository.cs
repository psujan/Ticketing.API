using Ticketing.API.Model.Domain;

namespace Ticketing.API.Repositories.Interfaces
{
    public interface IFileRepository
    {
        Task<IEnumerable<Model.Domain.File>> UploadFiles(List<IFormFile> files , string? model, string ? uploadDir , int? modelId);

        Task<Model.Domain.File> UploadFile(IFormFile file, string? model , string? uploadDir , int ? modelId);

        void DeleteFile(string uploadDir, string fileName);
    }
}
