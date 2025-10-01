namespace Core.Entites;
public class Semester
{
    public int SemesterID { get; set; }
    public string SemesterName { get; set; } = string.Empty;
    public ICollection<Course> Courses { get; set; } = new List<Course>();
}
