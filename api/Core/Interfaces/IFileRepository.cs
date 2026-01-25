using Core.Entities;
using Core.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;
public interface IFileRepository : IGenericRepository<Core.Entities.File>
{
    Task<IEnumerable<Core.Entities.File>> GetFilesByCourseAsync(int courseId , int? DocTypeId);
    Task<int> IncrementDownloadCountAsync(int fileId);

}