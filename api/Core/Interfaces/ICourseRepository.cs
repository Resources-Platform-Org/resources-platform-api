using Core.Entities;

public interface ICourseRepository : IGenericRepository<Course>
{
    Task<Course?> GetDetailsAsync(int courseId);
    Task<Course?> SearchAsync(string term);
    
}