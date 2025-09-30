using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entites;
public class University
{
    public Guid UniversityID { get; set; } = Guid.NewGuid();

    public string UniversityName { get; set; } = string.Empty;

    public ICollection<Major> Majors { get; set; } = new List<Major>();
}
