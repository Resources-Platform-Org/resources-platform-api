using System;
using Core.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
namespace Infrastructure.Repository;

public class ResourceRepository : GenericRepository<Resource>, IResourceRepository
{
    private readonly ApplicationDbContext _context;
    public ResourceRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }
    public async Task<IEnumerable<Resource>> GetResourcesByCourseAsync(int courseId, int? DocTypeId)
    {
        var query = _context.Resources
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

    public async Task<int> IncrementDownloadCountAsync(int resourceId)
    {
        // Using ExecuteUpdate for atomicity and performance (EF Core 7+)
        return await _context.Resources
            .Where(x => x.Id == resourceId)
            .ExecuteUpdateAsync(setters => setters.SetProperty(f => f.DownloadsCount, f => f.DownloadsCount + 1));
    }
}