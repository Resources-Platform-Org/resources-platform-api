using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public UnitOfWork(ApplicationDbContext context, IUserRepository users,
        IGenericRepository<University> universities,
        IResourceRepository resources, ICourseRepository courses,
        IGenericRepository<Professor> professors,
        IGenericRepository<DocumentType> documentTypes,
        IGenericRepository<CourseMajor> majorUniversities,
        IGenericRepository<Major> majors
        )
    {
        _context = context;
        Users = users;
        Universities = universities;
        Resources = resources;
        Courses = courses;
        Professors = professors;
        DocumentsType = documentTypes;
        CoursesMajors = majorUniversities;
        Majors = majors;
    }

    public IUserRepository Users { get; }
    public ICourseRepository Courses { get; }
    public IResourceRepository Resources { get; }
    public IGenericRepository<University> Universities { get; }
    public IGenericRepository<DocumentType> DocumentsType { get; }

    public IGenericRepository<CourseMajor> CoursesMajors { get; }
    public IGenericRepository<Professor> Professors { get; }
    public IGenericRepository<Major> Majors { get; }


    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}