namespace Core.Entities;
public class DocumentType
{
    public int DocumentTypeID { get; set; }
    public enDocument TypeName { get; set; }
    public ICollection<File> Files { get; set; } = new List<File>();
}
