namespace Core.Entites;
public class Course
{
    public int CourseID { get; set; }
    public string CourseName { get; set; } = string.Empty;
    public string CourseCode { get; set; } = string.Empty;
    // Foreign Keys
    public int MajorID { get; set; }
    public int LevelID { get; set; }
    public int SemesterID { get; set; }
    // Navigation Properties
    public Major? Major { get; set; }
    public AcademicLevel AcademicLevel { get; set; } = default!;
    public Semester Semester { get; set; } = default!;
    public ICollection<File> Files { get; set; } = new List<File>();
    public ICollection<CourseProfessor> CourseProfessors { get; set; } = new List<CourseProfessor>();
}
