using Core.Entities;

public interface IAcademicLevelRepository
{
    Task<IEnumerable<Course>> GetCoursesAsync(int levelId, int? semesterId);
}