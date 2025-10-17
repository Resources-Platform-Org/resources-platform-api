
using Core.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    class SemesterRepository : GenericRepository<Semester>, ISemesterRepository
    {
        private readonly ApplicationDbContext _context;
        public SemesterRepository(ApplicationDbContext context) : base(context)
        {
            _context = context ;
        }
        public async Task<IEnumerable<Course>> GetCoursesBySemesterAsync(int semesterId)
        {
            return await _context.Courses
                .Where(c => c.SemesterID == semesterId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Professor>> GetProfessorsAsync(int semesterId)
        {
            // Return distinct professors who are assigned to courses in the requested semester.
            return await _context.CourseProfessors
                .Include(cp => cp.Course)
                .Include(cp => cp.Professor)
                .Where(cp => cp.Course.SemesterID == semesterId)
                .Select(cp => cp.Professor)
                .Distinct()
                .ToListAsync();
        }
    }
}
