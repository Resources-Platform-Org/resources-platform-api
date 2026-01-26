namespace Core.Entities;

public class DocumentType
{
    public int Id { get; set; }
    public enDocument Name { get; set; }
    public ICollection<Resource> Resources { get; set; } = new List<Resource>();
}
