using Core.Entities;

public interface ISemesterRepository : IGenericRepository<Semester>
{
    Task<IEnumerable<Course>> GetCoursesBySemesterAsync(int semesterId);
    Task<IEnumerable<Professor>> GetProfessorsAsync(int semesterId);
}