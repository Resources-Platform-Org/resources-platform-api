using Core.Entities;

public interface ICourseRepository : IGenericRepository<Course>
{
    Task<Course?> GetCourseWithDetailsAsync(int courseId);
    Task<IEnumerable<Course>> GetCoursesByLevelAndMajorAsync(int majorId, int levelId);
}