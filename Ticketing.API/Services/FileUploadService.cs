using Ticketing.API.Model;

namespace Ticketing.API.Services
{
    public class FileUploadService :IFileUploadService
    {
        public readonly static string Default_Upload_Dir = "Uploads";

        private readonly IHostEnvironment environment;

        public FileUploadService(IHostEnvironment env)
        {
            environment = env;
        }

        public async Task<UploadFile ?> UploadFile(IFormFile file , string? modelName , string ? uploadDir)
        {
            try
            {
                if (file.Length <= 0)
                {
                    return null;
                }

                uploadDir = uploadDir ?? Default_Upload_Dir;

                string storagePath = Path.Combine(environment.ContentRootPath, uploadDir);

                // Create the directory if it doesn't exist
                if (!Directory.Exists(storagePath))
                {
                    Directory.CreateDirectory(storagePath);
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
                    Path = uploadDir  + renamed 
                };
            }
            catch (Exception ex) 
            {
                return null;  
            }
        }
    }
}
