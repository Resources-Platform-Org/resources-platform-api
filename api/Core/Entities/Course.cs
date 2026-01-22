namespace Core.Entities;
public class Course
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;

    // Navigation Properties
    public ICollection<CourseMajor> CourseMajors { get; set; } = new List<CourseMajor>();
    public ICollection<Professor> Professors { get; set; } = new List<Professor>();
    public ICollection<File> Files { get; set; } = new List<File>();
  }
