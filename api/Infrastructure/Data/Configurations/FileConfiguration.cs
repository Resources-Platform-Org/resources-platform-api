using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Core.Entities;

namespace Infrastructure.Data.Configurations;

public class FileConfiguration : IEntityTypeConfiguration<Core.Entities.File>
{
    public void Configure(EntityTypeBuilder<Core.Entities.File> builder)
    {
        builder.HasKey(x => x.FileID);
        builder.Property(x => x.FileName)
            .HasMaxLength(50).IsRequired();
        builder.Property(x => x.FileType)
            .IsRequired();

        // Define Relationships between file and User
        // A User can have many Files, but a File belongs to one User
        builder.HasOne<User>(u => u.Uploader)
            .WithMany(f => f.UploadedFiles)
            .HasForeignKey(fu => fu.UploaderID);

        // Define Relationships between file and DocumentType
        // A DocumentType can have many Files, but a File belongs to one DocumentType
        builder.HasOne<DocumentType>(dt => dt.DocumentType)
        .WithMany(f => f.Files)
        .HasForeignKey(fd => fd.DocumentTypeID);

        // Define Relationships between file and Course
        // A Course can have many Files, but a File belongs to one Course
        builder.HasOne<Course>(c => c.Course)
            .WithMany(f => f.Files)
            .HasForeignKey(cf => cf.CourseID);

        // Define Relationships between file and Professor
        // A Professor can have many Files, but a File belongs to one Professor
        builder.HasOne<Professor>(p => p.Professor)
            .WithMany(f => f.Files)
            .HasForeignKey(pf => pf.ProfessorID);

    }
}
