using Core.Interfaces;

namespace Core.Entities;

public class Course : IAuditableEntity
{
  public int Id { get; set; }
  public string Name { get; set; } = string.Empty;
  public string Code { get; set; } = string.Empty;

  // Navigation Properties
  public ICollection<CourseMajor> CourseMajors { get; set; } = new List<CourseMajor>();
  public ICollection<Professor> Professors { get; set; } = new List<Professor>();
  public ICollection<Resource> Resources { get; set; } = new List<Resource>();

  // Auditing Properties
  public DateTime CreatedAt { get; set; }
  public DateTime? UpdatedAt { get; set; }
}
