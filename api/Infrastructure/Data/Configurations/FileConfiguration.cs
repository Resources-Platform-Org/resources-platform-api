using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Core.Entities;

namespace Infrastructure.Data.Configurations;

public class FileConfiguration : IEntityTypeConfiguration<Core.Entities.File>
{
    public void Configure(EntityTypeBuilder<Core.Entities.File> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name)
            .HasMaxLength(100)
            .IsRequired();
        builder.Property(x => x.Path)
            .HasMaxLength(500)
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
        // User <-> File
        builder.HasOne<User>(u => u.Uploader)
            .WithMany(f => f.UploadedFiles)
            .HasForeignKey(x => x.UploaderId)
            .OnDelete(DeleteBehavior.Restrict);

        // Course <-> File 
        builder.HasOne<Course>(c => c.Course)
            .WithMany(f => f.Files)
            .HasForeignKey(x => x.CourseId)
            .OnDelete(DeleteBehavior.Restrict);

        // DocumentType <-> File
        builder.HasOne<DocumentType>(d => d.DocumentType)
            .WithMany(f => f.Files)
            .HasForeignKey(x => x.DocumentTypeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
