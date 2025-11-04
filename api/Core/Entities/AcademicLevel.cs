namespace Core.Entities;
public class AcademicLevel
{
    public int LevelID { get; set; }
    public enLevel LevelName { get; set; }
    public ICollection<Course> Courses { get; set; } = new List<Course>();
}
