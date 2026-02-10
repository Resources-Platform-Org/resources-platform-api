using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Core.Interfaces;

public interface IFileService
{
    Task<string> UploadFile(IFormFile file, string folderName);
    void DeleteFile(string fileName, string folderName);
    string GetFilePath(string fileName, string folderName);
}
