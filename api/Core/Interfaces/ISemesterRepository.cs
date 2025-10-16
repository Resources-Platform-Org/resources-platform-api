using Core.Entities;

public interface ISemesterRepository
{
    Task<IEnumerable<Course>> GetCoursesBySemesterAsync(int semesterId);
    Task<IEnumerable<Professor>> GetProfessorsAsync(int semesterId);
}