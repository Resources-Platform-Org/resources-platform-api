using Core.Entities;
public interface IFileRepository
{
    Task<IEnumerable<Core.Entities.File>> GetLatestForCourseAsync(int courseId);
    Task<IEnumerable<Core.Entities.File>> SearchAsync(int courseId, int? professorId, enDocument documentTypeId);
}