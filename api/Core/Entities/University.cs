namespace Core.Entities;
public class University
{
    public int UniversityID { get; set; }
    public string UniversityName { get; set; } = string.Empty;
    public ICollection<Major> Majors { get; set; } = new List<Major>();
}
