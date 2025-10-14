namespace Core.Entities;

public class CourseProfessor
{
    // Foreign Keys [Composite Key]
    // middle table to connect professors and courses M <=> M
    public int CourseID { get; set; }
    public int ProfessorID { get; set; }
    // Navigation Properties
    public Professor Professor { get; set; } = default!;
    public Course Course { get; set; } = default!;
}