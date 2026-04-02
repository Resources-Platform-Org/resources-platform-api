namespace Api.Dtos.Resources;

public class ResourceResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int DownloadsCount { get; set; }
    public bool IsApproved { get; set; }
    public string CourseName { get; set; } = string.Empty;
    public string DocumentTypeName { get; set; } = string.Empty;
    public string UploaderName { get; set; } = string.Empty;
    public string Extension { get; set; } = string.Empty;
    public string DownloadUrl { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
