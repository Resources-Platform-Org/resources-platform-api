using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Core.Entities;

namespace Infrastructure.Data.Configurations;

public class CourseConfiguration : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).HasMaxLength(100).IsRequired();
        builder.Property(x => x.Code).HasMaxLength(100);

        // ----------- Relationship ---------------
        //-----------------------------------------

        builder.HasMany<Professor>(x => x.Professors)
            .WithMany(x => x.Courses)
            .UsingEntity(e => e.ToTable("CourseProfessor"));

    }
}
