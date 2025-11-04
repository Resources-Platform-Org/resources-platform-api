using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Core.Entities;

namespace Infrastructure.Data.Configurations;

public class CourseConfiguration : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        builder.HasKey(x => x.CourseID);
        builder.Property(x => x.CourseName)
            .HasMaxLength(100).IsRequired();
        builder.Property(x => x.CourseCode)
            .HasMaxLength(50).IsRequired();

        // Define relationships between Course and Major
        // A Major can have many Courses, but a Course belongs to one Major
        builder.HasOne<Major>(x => x.Major)
            .WithMany(m => m.Courses)
            .HasForeignKey(x => x.MajorID);

        // Define relationships between Semester and Course
        // A Semester can have many Courses, but a Course belongs to one Semester
        builder.HasOne<Semester>(s => s.Semester)
            .WithMany(c => c.Courses)
            .HasForeignKey(x => x.SemesterID);

        // Define Relationships between Level and Course
        // A Level can have many Courses, but a Course belongs to one Level
        builder.HasOne<AcademicLevel>(l => l.AcademicLevel)
            .WithMany(c => c.Courses)
            .HasForeignKey(x => x.LevelID);

    }
}
