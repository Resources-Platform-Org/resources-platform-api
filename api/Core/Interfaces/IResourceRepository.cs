using Core.Entities;
using Core.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;
public interface IResourceRepository : IGenericRepository<Resource>
{
    Task<IEnumerable<Resource>> GetResourcesByCourseAsync(int courseId, int? DocTypeId);
    Task<int> IncrementDownloadCountAsync(int resourceId);

}