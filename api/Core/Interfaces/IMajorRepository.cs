using Core.Entities;

public interface IMajorUniversity
{
    Task<IEnumerable<Course>> GetCoursesAsync(int majorId, int? semesterId, int? levelId);
    Task<bool> IsNameTakenAsync(string name);
}