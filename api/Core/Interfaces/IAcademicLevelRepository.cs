using Core.Entities;

public interface IAcademicLevelRepository : IGenericRepository<AcademicLevel>
{
    Task<IEnumerable<Course>> GetCoursesAsync(int levelId, int? semesterId);
}