using Core.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    class FileRepository : GenericRepository<Core.Entities.File>, IFileRepository
    {
        private readonly ApplicationDbContext _context ;
        public FileRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Core.Entities.File>> GetLatestForCourseAsync(int courseId)
        {
            return await _context.Files
                .AsNoTracking()
                .Where(c => c.CourseID == courseId)
                .OrderByDescending(f => f.UploadDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Core.Entities.File>> SearchAsync(int courseId, int? professorId, enDocument documentTypeId)
        {
            var query = _context.Files
                .AsNoTracking()
                .Where(f => f.CourseID == courseId && documentTypeId == f.DocumentType.TypeName);

            if (professorId.HasValue)
            {
                query = query.Where(p => professorId == p.ProfessorID);
            }
            return await query.ToListAsync();
        }
    }
}
