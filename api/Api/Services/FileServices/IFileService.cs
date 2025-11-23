using Api.Dtos.Files;
using Core.Entities;
using Core.Models.Files;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Services.FileServices
{
    public interface IFileService
    {
        Task<Core.Entities.File> CreateMetadataAsync(CreateFileDto dto, string savedFileName);
        Task<Core.Entities.File?> GetByIdAsync(int id);
        Task<(IEnumerable<Core.Entities.File> Items, int TotalCount)> FilterAsync(FileFilterDto filter);
        Task<IEnumerable<Core.Entities.File>> GetRecentAsync(int count);
        Task DeleteAsync(int id);
    }
}
