using Core.Enums;

namespace Core.Entities;
public class File
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Path { get; set; } = string.Empty;
    public int DownloadsCount { get; set; }
    public bool IsApproved { get; set; }
    public enExtension Extension { get; set; }
    
    // Foreign Keys
    public int DocumentTypeId {get; set;}
    public int CourseId {get; set;}
    public int UploaderId {get; set;}

    // Navigation Properties
    public DocumentType? DocumentType { get; set; }
    public Course? Course { get; set; }
    public User? Uploader { get; set; }
}
