using Core.Entities;
using Core.Enums;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class FileRepository : GenericRepository<Core.Entities.File>, IFileRepository
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

        public async Task<IEnumerable<Core.Entities.File>> SearchAsync(int courseId, int? professorId, enDocument documentType)
        {
            var query = _context.Files
                .AsNoTracking()
                .Include(f => f.Course).ThenInclude(c => c.Major).ThenInclude(m => m.University)
                .Include(f => f.Professor)
                .Include(f => f.DocumentType)
                .Where(f => f.CourseID == courseId && documentType == f.DocumentType.TypeName);

            if (professorId.HasValue)
            {
                query = query.Where(p => p.ProfessorID == professorId.Value);
            }

            return await query
                .OrderByDescending(f => f.UploadDate)
                .ToListAsync();
        }

        public async Task<(IReadOnlyList<Core.Entities.File> Items, int TotalCount)> FilterAsync(
            int? universityId,
            int? majorId,
            int? courseId,
            int? academicLevelId,
            int? semesterId,
            int? documentTypeId,
            int? professorId,
            string? search,
            string? sort,
            int page,
            int pageSize)
        {
            var files = _context.Files.AsNoTracking()
                .Include(f => f.Course).ThenInclude(c => c.Major).ThenInclude(m => m.University)
                .Include(f => f.Course).ThenInclude(c => c.AcademicLevel)
                .Include(f => f.Course).ThenInclude(c => c.Semester)
                .Include(f => f.Professor)
                .Include(f => f.DocumentType)
                .AsQueryable();

            if (courseId.HasValue)
                files = files.Where(f => f.CourseID == courseId.Value);
            if (majorId.HasValue)
                files = files.Where(f => f.Course.MajorID == majorId.Value);
            if (universityId.HasValue)
                files = files.Where(f => f.Course.Major.UniversityID == universityId.Value);
            if (academicLevelId.HasValue)
                files = files.Where(f => f.Course.LevelID == academicLevelId.Value);
            if (semesterId.HasValue)
                files = files.Where(f => f.Course.SemesterID == semesterId.Value);
            if (documentTypeId.HasValue)
                files = files.Where(f => f.DocumentTypeID == documentTypeId.Value);
            if (professorId.HasValue)
                files = files.Where(f => f.ProfessorID == professorId.Value);
            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = search.Trim().ToLower();
                files = files.Where(f => f.FileName.ToLower().Contains(term) || f.Course.CourseName.ToLower().Contains(term));
            }

            files = sort?.ToLower() == "oldest" ? files.OrderBy(f => f.UploadDate) : files.OrderByDescending(f => f.UploadDate);

            var total = await files.CountAsync();
            var items = await files.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            return (items, total);
        }
    }
}
