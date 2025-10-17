using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Core.Entities;

namespace Infrastructure.Data.Configurations;

public class MajorConfiguration : IEntityTypeConfiguration<Major>
{
    public void Configure(EntityTypeBuilder<Major> builder)
    {
        builder.HasKey(x => x.MajorID);
        builder.Property(x => x.MajorName)
        .HasMaxLength(100).IsRequired();

        // Define relationships between University and Major
        // A University can have many Majors, but a Major belongs to one University
        builder.HasOne<University>(u => u.University)
            .WithMany(m => m.Majors)
            .HasForeignKey(x => x.UniversityID);
    }
}
