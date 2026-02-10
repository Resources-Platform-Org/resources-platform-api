using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Core.Entities;

namespace Infrastructure.Data.Configurations;

public class ResourceConfiguration : IEntityTypeConfiguration<Resource>
{
    public void Configure(EntityTypeBuilder<Resource> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name)
            .HasMaxLength(100)
            .IsRequired();
        builder.Property(x => x.Path)
            .HasMaxLength(500)
            .IsRequired();
        builder.Property(x => x.StoredFileName)
            .IsRequired();
        builder.Property(x => x.DownloadsCount)
            .HasDefaultValue(0);
        builder.Property(x => x.IsApproved)
            .HasDefaultValue(false);
        builder.Property(x => x.Extension)
            .HasConversion<byte>()
            .IsRequired();

        // ------------- Realtionships ------------
        //-----------------------------------------
        // User <-> Resource
        builder.HasOne<User>(u => u.Uploader)
            .WithMany(u => u.UploadedResources)
            .HasForeignKey(x => x.UploaderId)
            .OnDelete(DeleteBehavior.Restrict);

        // Course <-> Resource 
        builder.HasOne<Course>(c => c.Course)
            .WithMany(c => c.Resources)
            .HasForeignKey(x => x.CourseId)
            .OnDelete(DeleteBehavior.Restrict);

        // DocumentType <-> Resource
        builder.HasOne<DocumentType>(d => d.DocumentType)
            .WithMany(d => d.Resources)
            .HasForeignKey(x => x.DocumentTypeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
