using Core.Enums;
using Core.Interfaces;

namespace Core.Entities;

public class Resource : IAuditableEntity, ISoftDeletable
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Path { get; set; } = string.Empty;
    public int DownloadsCount { get; set; }
    public bool IsApproved { get; set; }
    public enExtension Extension { get; set; }
    public string StoredFileName { get; set; } = string.Empty;

    // Foreign Keys
    public int DocumentTypeId { get; set; }
    public int CourseId { get; set; }
    public int UploaderId { get; set; }

    // Auditing Properties
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    // Soft Deletion Properties
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }

    // Navigation Properties
    public DocumentType? DocumentType { get; set; }
    public Course? Course { get; set; }
    public User? Uploader { get; set; }
}
