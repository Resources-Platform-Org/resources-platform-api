using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Core.Entites;

namespace Infrastructure.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.UserID);
        builder.Property(x => x.UserName)
            .HasMaxLength(50).IsRequired();
        builder.Property(x => x.Email)
            .HasMaxLength(100).IsRequired();
        builder.Property(x => x.PasswordHash)
            .HasMaxLength(50).IsRequired();
        builder.Property(x => x.Role)
            .IsRequired();
    }
}
