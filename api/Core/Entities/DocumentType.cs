using Core.Interfaces;

namespace Core.Entities;

public class DocumentType : IAuditableEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    // Auditing Properties
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public ICollection<Resource> Resources { get; set; } = new List<Resource>();
}
