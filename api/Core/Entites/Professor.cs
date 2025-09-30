using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entites;
public class Professor
{
    public Guid ProfessorID { get; set; } = Guid.NewGuid();

    public string ProfessorName { get; set; } = string.Empty;

    public ICollection<File> Files { get; set; } = new List<File>();

    public ICollection<CourseProfessor> CourseProfessors { get; set; } = new List<CourseProfessor>();
}
