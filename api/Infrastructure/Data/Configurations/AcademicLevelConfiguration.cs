using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Core.Entites;

namespace Infrastructure.Data.Configurations;

public class AcademicLevelConfiguration : IEntityTypeConfiguration<AcademicLevel>
{
    public void Configure(EntityTypeBuilder<AcademicLevel> builder)
    {
        builder.HasKey(x => x.LevelID);
        builder.Property(x => x.LevelName)
        .IsRequired();
    }
}
