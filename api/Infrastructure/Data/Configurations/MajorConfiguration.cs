using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Core.Entities;

namespace Infrastructure.Data.Configurations;

public class MajorConfiguration : IEntityTypeConfiguration<Major>
{
    public void Configure(EntityTypeBuilder<Major> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).HasMaxLength(50)
            .IsRequired();

        // Realtionships :-
        builder.HasOne<University>(x => x.University)
            .WithMany(x => x.Majors)
            .HasForeignKey(x => x.UniversityId);
    }
}
