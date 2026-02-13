using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class CourseMajorConfiguration : IEntityTypeConfiguration<CourseMajor>
{
    public void Configure(EntityTypeBuilder<CourseMajor> builder)
    {
        builder.HasKey(x => new { x.MajorId, x.CourseId });

        // dealing with byte in enum is effecient .
        builder.Property(x => x.Semester)
            .HasConversion<byte>()
            .IsRequired();

        builder.Property(x => x.Level)
            .HasConversion<byte>()
            .IsRequired();

        // ---------- Realtionships -------------
        //---------------------------------------

        // Course => CourseMajor 
        builder.HasOne(x => x.Course)
            .WithMany(x => x.CourseMajors)
            .HasForeignKey(x => x.CourseId)
            .OnDelete(DeleteBehavior.Restrict);

        // Major => CourseMajor 
        builder.HasOne(x => x.Major)
            .WithMany(x => x.CourseMajors)
            .HasForeignKey(x => x.MajorId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
