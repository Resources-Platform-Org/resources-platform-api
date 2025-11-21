using Core.Entities;
using Core.Enums;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class FileRepository : GenericRepository<Core.Entities.File>, IFileRepository
    {
        private readonly ApplicationDbContext _context;

        public FileRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        private IQueryable<Core.Entities.File> BaseFilesQuery(bool noTracking = true)
        {
            var query = _context.Files.AsQueryable();

            if (noTracking)
                query = query.AsNoTracking();

            return query
                .Include(f => f.Course)
                    .ThenInclude(c => c.Major)
                        .ThenInclude(m => m.University)

                .Include(f => f.Professor)
                .Include(f => f.DocumentType)
                .Include(f => f.Uploader);
        }

        public async Task<IEnumerable<Core.Entities.File>> GetLatestForCourseAsync(int courseId)
        {
            return await BaseFilesQuery()
                .Where(f => f.CourseID == courseId)
                .OrderByDescending(f => f.UploadDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Core.Entities.File>> SearchAsync(int courseId, int? professorId, enDocument documentType)
        {
            var query = BaseFilesQuery()
                .Where(f =>
                    f.CourseID == courseId &&
                    f.DocumentType.TypeName == documentType
                );

            if (professorId.HasValue)
                query = query.Where(f => f.ProfessorID == professorId.Value);

            return await query
                .OrderByDescending(f => f.UploadDate)
                .ToListAsync();
        }

        public async Task<(IReadOnlyList<Core.Entities.File> Items, int TotalCount)> FilterAsync(
            int? universityId,
            int? majorId,
            int? courseId,
            int? documentTypeId,
            int? professorId,
            string? search,
            string? sort,
            int page,
            int pageSize)
        {
            var query = BaseFilesQuery();

            // Filtering based on existing fields only
            if (courseId.HasValue)
                query = query.Where(f => f.CourseID == courseId.Value);

            if (majorId.HasValue)
                query = query.Where(f => f.Course.MajorID == majorId.Value);

            if (universityId.HasValue)
                query = query.Where(f => f.Course.Major.UniversityID == universityId.Value);

            if (documentTypeId.HasValue)
                query = query.Where(f => f.DocumentTypeID == documentTypeId.Value);

            if (professorId.HasValue)
                query = query.Where(f => f.ProfessorID == professorId.Value);

            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = search.Trim().ToLower();
                query = query.Where(f =>
                    f.FileName.ToLower().Contains(term) ||
                    f.Course.CourseName.ToLower().Contains(term)
                );
            }

            var ordered = (sort ?? "").Trim().ToLower() switch
            {
                "oldest" => query.OrderBy(f => f.UploadDate),
                _ => query.OrderByDescending(f => f.UploadDate),
            };

            var total = await ordered.CountAsync();

            page = page <= 0 ? 1 : page;
            pageSize = pageSize <= 0 ? 10 : pageSize;

            var items = await ordered
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, total);
        }
    }
}
