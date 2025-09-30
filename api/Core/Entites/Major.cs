using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entites;
public class Major
{
    public Guid MajorID { get; set; } = Guid.NewGuid();
    public string MajorName { get; set; } = string.Empty;

    public Guid UniversityID {  get; set; }
    public University University { get; set; } = default!;

    public ICollection<Course> Courses { get; set; } = new List<Course>();
}
