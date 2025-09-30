using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entites;
public class Course
{
    public Guid CourseID { get; set; } = Guid.NewGuid();

    public string CourseName { get; set; } = string.Empty;

    public string CourseCode { get; set; } = string.Empty;

    public Guid MajorID { get; set; }
    public Major Major { get; set; } = default!;

    public Guid LevelID { get; set; }
    public AcademicLevel AcademicLevel { get; set; } = default!;

    public Guid SemesterID { get; set; }
    public Semester Semester { get; set; } = default!;

    public ICollection<File> Files { get; set; } = new List<File>();

    public ICollection<CourseProfessor> CourseProfessors { get; set; } = new List<CourseProfessor>();
}
