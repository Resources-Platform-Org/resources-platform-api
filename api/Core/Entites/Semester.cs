using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entites;
public class Semester
{
    public Guid SemesterID {  get; set; } = Guid.NewGuid();

    public string SemesterName { get; set; } = string.Empty;

    public ICollection<Course> courses { get;set; } = new List<Course>();
}
