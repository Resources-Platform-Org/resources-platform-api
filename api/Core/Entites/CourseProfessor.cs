using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entites;
public class CourseProfessor
{
    public Guid CourseID { get; set; }
    public Course Course { get; set; } = default!;
   
    public Guid ProfessorID { get; set; }
    public Professor Professor { get; set; } = default!;


}