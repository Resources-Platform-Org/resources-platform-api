namespace Core.Entites;
public class CourseProfessor
{
    public int CourseID { get; set; }
    public Course Course { get; set; } = default!;
    public int ProfessorID { get; set; }
    public Professor Professor { get; set; } = default!;
}