using Core.Entities;
using Core.Enums;

namespace Core.Interfaces
{
    public interface IDocumentTypeRepository : IGenericRepository<DocumentType>
    {
        Task<Dictionary<enFileType,int>> CountFileByTypeForCourseAsync(int courseId);
    }
}