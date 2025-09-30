using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entites;
public class DocumentType
{
    public Guid DocumentTypeID { get; set; } = Guid.NewGuid();
    public string TypeName { get; set; } = string.Empty;

    public ICollection<File> Files { get; set; } = new List<File>();
}
