namespace Core.Entites;
public class File
{
    public int FileID { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string FileType { get; set; } = string.Empty;
    public DateOnly UploadDate { get; set; }
    public int UploaderID { get; set; }
    public User Uploader { get; set; } = default!;
    public int CourseID { get; set; }
    public Course Course { get; set; } = default!;
    public int ProfessorID { get; set; }
    public Professor Professor { get; set; } = default!;
    public int DocumentTypeID { get; set; }
    public DocumentType DocumentType { get; set; } = default!;
}
