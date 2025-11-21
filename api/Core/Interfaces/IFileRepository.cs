using Core.Entities;
using Core.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;
public interface IFileRepository : IGenericRepository<Core.Entities.File>
{
    Task<IEnumerable<Core.Entities.File>> GetLatestForCourseAsync(int courseId);
    Task<IEnumerable<Core.Entities.File>> SearchAsync(int courseId, int? professorId, enDocument documentTypeId);
    // Returns filtered, sorted and paginated files plus total count for given filters.
    Task<(IReadOnlyList<Core.Entities.File> Items, int TotalCount)> FilterAsync(
        int? universityId,
        int? majorId,
        int? courseId,
        int? documentTypeId,
        int? professorId,
        string? search,
        string? sort,
        int page,
        int pageSize);
}