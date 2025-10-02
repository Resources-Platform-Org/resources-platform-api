namespace Core.Entites;
public class Major
{
    public int MajorID { get; set; }
    public string MajorName { get; set; } = string.Empty;
    // Foreginkey :-
    public int UniversityID { get; set; }
    // Navigation Properties
    public University University { get; set; } = default!;
    public ICollection<Course> Courses { get; set; } = new List<Course>();
}