using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entites;
public class File
{
    public Guid FileID { get; set; } = Guid.NewGuid();

    public string FileName { get; set; } = string.Empty;

    public string FileType { get; set; } = string.Empty;

    public DateOnly UploadDate { get; set; }

    public Guid CourseID { get; set; }
    public Course Course { get; set; } = default!;

    public Guid ProfessorID { get; set; }
    public Professor Professor { get; set; } = default!;

    public Guid DocumentTypeID { get; set; }
    public DocumentType DocumentType { get; set; } = default!;

    public Guid UploaderID { get; set; }
    public User User { get; set; } = default!;

}
