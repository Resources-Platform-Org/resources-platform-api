using Core.Entities;

public interface IMajorUniversityRepository : IGenericRepository<Major>
{
    Task<IEnumerable<Course>> GetCoursesAsync(int majorId, int? semesterId, int? levelId);
    Task<bool> IsNameTakenAsync(string name);
}