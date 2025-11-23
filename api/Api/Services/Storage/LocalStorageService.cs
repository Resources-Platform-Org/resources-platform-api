using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Services.Storage
{
    public class LocalStorageService : IStorageService
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _uploadsFolder;

        public LocalStorageService(IWebHostEnvironment environment, IHttpContextAccessor httpContextAccessor)
        {
            _environment = environment;
            _httpContextAccessor = httpContextAccessor;
            _uploadsFolder = Path.Combine(_environment.ContentRootPath, "Resources", "Uploads");
            
            if (!Directory.Exists(_uploadsFolder))
                Directory.CreateDirectory(_uploadsFolder);
        }

        public async Task<string> SaveFileAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File is empty.");

            if (file.Length > 20 * 1024 * 1024)
                throw new ArgumentException("File size exceeds 20 MB.");

            var allowedExtensions = new[] { ".pdf", ".docx", ".pptx", ".jpg", ".jpeg", ".png" };
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(extension))
                throw new ArgumentException("Invalid file type. Allowed: pdf, docx, pptx, jpg, jpeg, png.");

            var uniqueFileName = $"{Guid.NewGuid()}_{file.FileName}";
            var filePath = Path.Combine(_uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return uniqueFileName;
        }

        public async Task<byte[]> GetFileBytesAsync(string fileName)
        {
            var filePath = Path.Combine(_uploadsFolder, fileName);
            if (!File.Exists(filePath))
                throw new FileNotFoundException("File not found.");

            return await File.ReadAllBytesAsync(filePath);
        }

        public void DeleteFile(string fileName)
        {
            var filePath = Path.Combine(_uploadsFolder, fileName);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        public bool FileExists(string fileName)
        {
            return File.Exists(Path.Combine(_uploadsFolder, fileName));
        }

        public string GetPublicUrl(string fileName)
        {
            var request = _httpContextAccessor.HttpContext?.Request;
            if (request == null) return $"/uploads/{fileName}";
            return $"{request.Scheme}://{request.Host}/uploads/{fileName}";
        }
    }
}
