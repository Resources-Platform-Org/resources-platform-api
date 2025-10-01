namespace Core.Entites;
public class Professor
{
    public int ProfessorID { get; set; }
    public string ProfessorName { get; set; } = string.Empty;
    public ICollection<File> Files { get; set; } = new List<File>();
    public ICollection<CourseProfessor> CourseProfessors { get; set; } = new List<CourseProfessor>();
}
