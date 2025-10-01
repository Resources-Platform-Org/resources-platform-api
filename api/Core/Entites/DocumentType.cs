namespace Core.Entites;
public class DocumentType
{
    public int DocumentTypeID { get; set; }
    public string TypeName { get; set; } = string.Empty;
    public ICollection<File> Files { get; set; } = new List<File>();
}
