using Core.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    class CourseRepository : GenericRepository<Course>, ICourseRepository
    {
        private readonly ApplicationDbContext _context;
        public CourseRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<Course> GetDetailsAsync(int courseId)
        {
            return await _context.Courses
                .Include(c => c.Major)
                .Include(c => c.AcademicLevel)
                .Include(c => c.Semester)
                .Include(c => c.Files)
                .Include(c => c.CourseProfessors)
                    .ThenInclude(cp => cp.Professor)
                .FirstOrDefaultAsync(c => c.CourseID == courseId) ?? new Course();
        }

        public async Task<Course> SearchAsync(object term) // term can be CourseName or CourseCode
        {
            return await _context.Courses
                .FirstOrDefaultAsync(c => c.CourseName == term.ToString() || c.CourseCode == term.ToString()) ?? new Course();
        }
    }
}
