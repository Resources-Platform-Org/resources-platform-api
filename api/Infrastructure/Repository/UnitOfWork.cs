using Core.Interfaces;
using Infrastructure.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public UnitOfWork(ApplicationDbContext context, IUserRepository users,
        IUniversityRepository universities,
        IFileRepository files, ICourseRepository courses,
        IProfessorRepository professors,
        IDocumentTypeRepository documentTypes,
        IMajorUniversityRepository majorUniversities,
        ISemesterRepository semesters,
        IAcademicLevelRepository academicLevels)
    {
        _context = context;
        Users = users;
        Universities = universities;
        Files = files;
        Courses = courses;
        Professors = professors;
        DocumentTypes = documentTypes;
        MajorUniversities = majorUniversities;
        Semesters = semesters;
        AcademicLevels = academicLevels;
    }

    public IUserRepository Users { get; }

    public IUniversityRepository Universities { get; }

    public IFileRepository Files { get; }

    public ICourseRepository Courses { get; }

    public IProfessorRepository Professors { get; }

    public IDocumentTypeRepository DocumentTypes { get; }

    public IMajorUniversityRepository MajorUniversities { get; }

    public ISemesterRepository Semesters { get; }

    public IAcademicLevelRepository AcademicLevels { get; }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}