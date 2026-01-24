namespace Core.Entities;
public class Major
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    // Foreign key :-
    public int UniversityId { get; set; }

    // Navigation Properties
    public University University { get; set; } = default!;
    public ICollection<CourseMajor> CourseMajors { get; set; } = new List<CourseMajor>();
}