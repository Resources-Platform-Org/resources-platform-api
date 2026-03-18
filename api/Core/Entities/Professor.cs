using Core.Interfaces;

namespace Core.Entities;

public class Professor : IAuditableEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;

    // Auditing Properties
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    // Navigation Properties
    public ICollection<Course> Courses { get; set; } = new List<Course>();
}
