using Core.Entities;
using Core.Enums;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class DocumentTypeRepository : GenericRepository<DocumentType>, IDocumentTypeRepository
    {
        private readonly ApplicationDbContext _context;
        public DocumentTypeRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public Task<Dictionary<enFileType, int>> CountFileByTypeForCourseAsync(int courseId)
        {
            var result = _context.Files
                .AsNoTracking()
                .Where(dt => dt.CourseID == courseId)
                .GroupBy(dt => dt.FileType)
                .Select(g => new { FileType = g.Key, Count = g.Count() })
                .ToDictionaryAsync(k => k.FileType, v => v.Count);

            return result;
        }
    }
}
