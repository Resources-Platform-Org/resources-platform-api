using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Core.Entities;

namespace Infrastructure.Data.Configurations;

public class CourseProfessorConfiguration : IEntityTypeConfiguration<CourseProfessor>
{
    public void Configure(EntityTypeBuilder<CourseProfessor> builder)
    {
        builder.HasKey(comp => new
        {
            comp.CourseID,
            comp.ProfessorID
        });

        // Define the relationships
        // This middle table (Junction table) that divide the M <=> M between Course and Professor
        
        // Each CourseProfessor has one Course
        builder.HasOne<Course>(c => c.Course)
            .WithMany(cp => cp.CourseProfessors)
            .HasForeignKey(c => c.CourseID);

        // Each CourseProfessor has one Professor
        builder.HasOne<Professor>(p => p.Professor)
            .WithMany(cp => cp.CourseProfessors)
            .HasForeignKey(p => p.ProfessorID);
    }
}
