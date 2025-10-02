using Core.Entites;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }

    // Add DbSet properties for entities :-
    public DbSet<University> Universities { get; set; }
    public DbSet<Major> Majors { get; set;}
    public DbSet<Course> Courses { get; set; }
    public DbSet<AcademicLevel> AcademicLevels { get; set; }
    public DbSet<Semester> Semesters { get; set; }
    public DbSet<Professor> Professors { get; set; }
    public DbSet<CourseProfessor> CourseProfessors { get; set; }
    public DbSet<Core.Entites.File> Files { get; set; }
    public DbSet<DocumentType> DocumentTypes { get; set; }
    public DbSet<User> Users { get; set; }

}
