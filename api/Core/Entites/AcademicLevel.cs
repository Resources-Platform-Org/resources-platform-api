using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entites;
public class AcademicLevel
{
    public Guid LevelID { get; set; } = Guid.NewGuid();

    public string LevelName { get; set; } = string.Empty;

    public ICollection<Course> courses { get; set; } = new List<Course>();
}
