using Core.Interfaces;

namespace Core.Entities;

public class User : IAuditableEntity, ISoftDeletable
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string? ProfilePicture { get; set; } = null;
    public enRoles Role { get; set; } = enRoles.Admin;

    // Auditing Properties
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    // Soft Deletion Properties
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    // Navigation Properties
    public ICollection<Resource> UploadedResources { get; set; } = new List<Resource>();
}
