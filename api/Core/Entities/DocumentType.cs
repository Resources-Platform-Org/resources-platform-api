namespace Core.Entities;
public class DocumentType
{
    public int Id { get; set; }
    public enDocument Name { get; set; }
    public ICollection<File> Files { get; set; } = new List<File>();
}
