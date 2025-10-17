using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IUnitOfWork
    {
        IUserRepository Users { get; }
        IUniversityRepository Universities { get; }
        IFileRepository Files { get; }
        ICourseRepository Courses { get; }
        IProfessorRepository Professors { get; }
        IDocumentTypeRepository DocumentTypes { get; }
        IMajorUniversityRepository MajorUniversities { get; }
        ISemesterRepository Semesters { get; }
        Task<int> SaveChangesAsync();
    }
}
