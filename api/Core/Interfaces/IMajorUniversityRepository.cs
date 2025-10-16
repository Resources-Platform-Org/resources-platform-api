using Core.Entities;

public interface IMajorUniversityRepository
{
    Task<IEnumerable<Course>> GetCoursesAsync(int majorId, int? semesterId, int? levelId);
    Task<bool> IsNameTakenAsync(string name);
}