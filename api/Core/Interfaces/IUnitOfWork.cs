using Core.Entities;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IUnitOfWork
    {
        IUserRepository Users { get; }
        IFileRepository Files { get; }
        ICourseRepository Courses { get; }
        IGenericRepository<University> Universities { get; }
        IGenericRepository<Professor> Professors { get; }
        IGenericRepository<Major> Majors { get; }
        IGenericRepository<DocumentType> DocumentsType { get; }
        Task<int> SaveChangesAsync();
    }
}
