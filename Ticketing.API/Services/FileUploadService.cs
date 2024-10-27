using Ticketing.API.Model;

namespace Ticketing.API.Services
{
    public class FileUploadService :IFileUploadService
    {
        public readonly static string Default_Upload_Dir = "Uploads";

        private readonly string storagePath;
        public FileUploadService(IHostEnvironment environment)
        {
            storagePath = Path.Combine(environment.ContentRootPath, Default_Upload_Dir);

            // Create the directory if it doesn't exist
            if (!Directory.Exists(storagePath))
            {
                Directory.CreateDirectory(storagePath); 
            }
        }

        public async Task<UploadFile ?> UploadFile(IFormFile file , string? modelName)
        {
            try
            {
                if (file.Length <= 0)
                {
                    return null;
                }

                string renamed = modelName != null ? modelName + "_" + Guid.NewGuid().ToString() + Path.GetExtension(file.FileName)
                                    : file.FileName + "_"+ Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

                var filePath = Path.Combine(storagePath, renamed);

                // Save the file to the specified path
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                return new UploadFile()
                {
                    FileName = renamed,
                    OriginalName =  file.FileName,
                    ByteSize =  file.Length,
                    Extension = Path.GetExtension(filePath),
                    Path =  filePath
                };
            }
            catch (Exception ex) 
            {
                return null;  
            }
        }
    }
}
