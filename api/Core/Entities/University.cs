using Core.Interfaces;

namespace Core.Entities;

public class University : IAuditableEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    // Auditing Properties
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    // Navigation property
    public ICollection<Major> Majors { get; set; } = new List<Major>();
}
