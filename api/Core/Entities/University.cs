namespace Core.Entities;
public class University
{
    public int ID { get; set; }
    public string Name { get; set; } = string.Empty;

    // Navigation property
    public ICollection<Major> Majors { get; set; } = new List<Major>();
}
