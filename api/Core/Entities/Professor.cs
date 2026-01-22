namespace Core.Entities;
public class Professor
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;

    // Navigation Properties
    public ICollection<Course> Courses { get; set; } = new List<Course>();
}
