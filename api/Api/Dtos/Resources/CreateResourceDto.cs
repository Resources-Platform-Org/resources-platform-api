using Microsoft.AspNetCore.Http;

namespace Api.Dtos.Resources;

public class CreateResourceDto
{
    public string Name { get; set; } = string.Empty;
    public int DocumentTypeId { get; set; }
    public int CourseId { get; set; }
    public int UploaderId { get; set; }
    public IFormFile File { get; set; } = null!;
}
