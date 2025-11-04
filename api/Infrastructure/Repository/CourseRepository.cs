using Core.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class CourseRepository : GenericRepository<Course>, ICourseRepository
    {
        private readonly ApplicationDbContext _context;
        public CourseRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<Course?> GetDetailsAsync(int courseId)
        {
            return await _context.Courses
                .AsNoTracking()
                .AsSplitQuery()
                .Include(c => c.Major)
                .Include(c => c.AcademicLevel)
                .Include(c => c.Semester)
                .Include(c => c.Files)
                .Include(c => c.CourseProfessors)
                    .ThenInclude(cp => cp.Professor)
                .FirstOrDefaultAsync(c => c.CourseID == courseId);
        }

        public async Task<Course?> SearchAsync(string term) // term can be CourseName or CourseCode
        {
            if (string.IsNullOrWhiteSpace(term))
            {
                return null;
            }

            var normalizedTerm = term.Trim();

            return await _context.Courses
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.CourseName == normalizedTerm || c.CourseCode == normalizedTerm);
        }
    }
}
