using Core.Entities;

public interface ICourseRepository
{
    Task<Course> GetDetailsAsync(int courseId);
    Task<Course> SearchAsync(object term);
    
}