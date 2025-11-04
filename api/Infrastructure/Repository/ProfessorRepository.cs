using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class ProfessorRepository : GenericRepository<Professor>, IProfessorRepository
    {
        private readonly ApplicationDbContext _context;
        public ProfessorRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<Course>> GetCoursesAsync(int professorId, int? semesterId = null, CancellationToken cancellationToken = default)
        {
            var query = _context.CourseProfessors
                .AsNoTracking()
                .Where(cp => cp.ProfessorID == professorId)
                .Select(cp => cp.Course);

            if (semesterId.HasValue)
            {
                query = query.Where(c => c.SemesterID == semesterId.Value);
            }

            return await query.ToListAsync(cancellationToken);
        }
    }
}
