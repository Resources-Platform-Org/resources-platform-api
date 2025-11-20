using Core.Entities;
using Core.Enums;
public interface IFileRepository : IGenericRepository<Core.Entities.File>
{
    Task<IEnumerable<Core.Entities.File>> GetLatestForCourseAsync(int courseId);
    Task<IEnumerable<Core.Entities.File>> SearchAsync(int courseId, int? professorId, enDocument documentTypeId);
    // Returns filtered, sorted and paginated files plus total count for given filters.
    Task<(IReadOnlyList<Core.Entities.File> Items, int TotalCount)> FilterAsync(
        int? universityId,
        int? majorId,
        int? courseId,
        int? academicLevelId,
        int? semesterId,
        int? documentTypeId,
        int? professorId,
        string? search,
        string? sort,
        int page,
        int pageSize);
}