namespace Core.Entites;
public class AcademicLevel
{
    public int LevelID { get; set; }
    public string LevelName { get; set; } = string.Empty;
    public ICollection<Course> Courses { get; set; } = new List<Course>();
}
