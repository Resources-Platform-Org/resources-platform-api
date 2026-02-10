using Core.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Infrastructure.Services;

public class FileServices : IFileService
{
    private readonly IWebHostEnvironment _environment;
    public FileServices(IWebHostEnvironment environment)
    {
        _environment = environment;
    }
    public async Task<string> UploadFile(IFormFile file, string folderName)
    {
        var uploadesFolder = Path.Combine(_environment.WebRootPath, folderName);

        if (Directory.Exists(uploadesFolder))
            Directory.CreateDirectory(uploadesFolder);

        var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
        var filePath = Path.Combine(uploadesFolder, uniqueFileName);

        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(fileStream);
        }
        return uniqueFileName;
    }
    public void DeleteFile(string fileName, string folderName)
    {
        var filePath = Path.Combine(_environment.WebRootPath, folderName, fileName);
        if (File.Exists(filePath))
            File.Delete(filePath);
    }

    public string GetFilePath(string fileName, string folderName)
    {
        return Path.Combine(_environment.WebRootPath, folderName, fileName);
    }
}
