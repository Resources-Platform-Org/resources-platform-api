namespace Core.Entites;
public class File
{
    public int FileID { get; set; }
    public string FileName { get; set; } = string.Empty;
    public enFileType FileType { get; set; }
    public DateOnly UploadDate { get; set; }
    // Foreign Keys
    public int UploaderID { get; set; }
    public int CourseID { get; set; }
    public int ProfessorID { get; set; }
    public int DocumentTypeID { get; set; }
    // Navigation Properties
    public User Uploader { get; set; } = default!;
    public Course Course { get; set; } = default!;
    public Professor Professor { get; set; } = default!;
    public DocumentType DocumentType { get; set; } = default!;
}
