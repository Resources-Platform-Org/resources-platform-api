using Core.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    class AcademicLevelRepository : GenericRepository<AcademicLevel>, IAcademicLevelRepository
    {
        private readonly ApplicationDbContext _context;
        public AcademicLevelRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Course>> GetCoursesAsync(int levelId, int? semesterId)
        {
            var query = _context.Courses
                .AsNoTracking()
                .Where(c => c.LevelID == levelId && (semesterId == null || c.SemesterID == semesterId));
            return await query.ToListAsync();
        }
    }
}
