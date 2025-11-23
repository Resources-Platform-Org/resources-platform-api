using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Api.Services.Storage
{
    public interface IStorageService
    {
        Task<string> SaveFileAsync(IFormFile file);
        Task<byte[]> GetFileBytesAsync(string fileName);
        void DeleteFile(string fileName);
        bool FileExists(string fileName);
        string GetPublicUrl(string fileName);
    }
}
