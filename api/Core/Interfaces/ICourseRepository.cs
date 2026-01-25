using Core.Entities;

public interface ICourseRepository : IGenericRepository<Course>
{
    Task<Course?> GetCourseWithDetailsAsync(int courseId);
    Task<Course?> GetCoursesByLevelAndMajorAsync(int majorId , int levelId);
    
}