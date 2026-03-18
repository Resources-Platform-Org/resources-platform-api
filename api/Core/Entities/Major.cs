using Core.Interfaces;

namespace Core.Entities;

public class Major : IAuditableEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    // Foreign key :-
    public int UniversityId { get; set; }

    // Auditing Properties
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    // Navigation Properties
    public University University { get; set; } = default!;
    public ICollection<CourseMajor> CourseMajors { get; set; } = new List<CourseMajor>();
}