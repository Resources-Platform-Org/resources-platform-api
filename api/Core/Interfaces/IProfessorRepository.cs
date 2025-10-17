using Core.Entities;

namespace Core.Interfaces
{
    public interface IProfessorRepository : IGenericRepository<Professor>
    {
        Task<IReadOnlyList<Course>> GetCoursesAsync(int professorId, int? semesterId = null, CancellationToken cancellationToken = default);
    }
}