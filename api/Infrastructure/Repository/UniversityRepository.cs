using Core.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class UniversityRepository : GenericRepository<University>, IUniversityRepository
    {
        private readonly ApplicationDbContext _context;
        public UniversityRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Major>> GetMajorsAsync(int universityId)
        {
            return await _context.Majors
                .AsNoTracking()
                .Where(x => x.UniversityID == universityId)
                .ToListAsync();
        }

        public async Task<bool> IsNameTakenAsync(string universityName)
        {
            return await _context.Universities
                .AnyAsync(u => u.UniversityName == universityName);
        }
    }
}
