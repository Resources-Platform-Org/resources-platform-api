using System;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
namespace Infrastructure.Repository;

public class FileRepository : GenericRepository<Core.Entities.File>, IFileRepository
{
    private readonly ApplicationDbContext _context;
    public FileRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }
    public async Task<IEnumerable<Core.Entities.File>> GetFilesByCourseAsync(int courseId, int? DocTypeId)
    {
        var query = _context.Files
            .AsNoTracking()
            .Include(f => f.DocumentType)
            .Include(f => f.Uploader)
            .Where(x => x.CourseId == courseId && x.IsApproved); // Assuming we only want approved files by default

        if (DocTypeId.HasValue)
        {
            query = query.Where(x => x.DocumentTypeId == DocTypeId.Value);
        }

        query = query.OrderByDescending(x => x.DownloadsCount);

        return await query.ToListAsync();
    }

    public async Task<int> IncrementDownloadCountAsync(int fileId)
    {
        // Using ExecuteUpdate for atomicity and performance (EF Core 7+)
        return await _context.Files
            .Where(x => x.Id == fileId)
            .ExecuteUpdateAsync(setters => setters.SetProperty(f => f.DownloadsCount, f => f.DownloadsCount + 1));
    }
}