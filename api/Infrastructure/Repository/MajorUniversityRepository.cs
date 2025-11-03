using Core.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class MajorUniversityRepository : GenericRepository<Major>, IMajorUniversityRepository
    {
        private readonly ApplicationDbContext _context;
        public MajorUniversityRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Course>> GetCoursesAsync(int majorId, int? semesterId, int? levelId)
        {
            var query = _context.Courses
                .AsNoTracking()
                .Where(c => c.MajorID == majorId);

            if (semesterId.HasValue)
                query = query.Where(c => c.SemesterID == semesterId.Value);
            if (levelId.HasValue)
                query = query.Where(c => c.LevelID == levelId.Value);

            return await query.ToListAsync();
        }

        public Task<bool> IsNameTakenAsync(string name)
        {
            return _context.Majors
                .AnyAsync(m => m.MajorName == name);
        }
    }
}
