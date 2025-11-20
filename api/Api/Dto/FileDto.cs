using System;
using Core.Enums;

namespace Api.Dto;
public class FileDto
{
    public int FileId { get; set; }
    public string FileName { get; set; } = string.Empty;
    public enFileType FileType { get; set; }
    public DateOnly UploadDate { get; set; }
    public SimpleEntityDto Course { get; set; } = new();
    public SimpleEntityDto Major { get; set; } = new();
    public SimpleEntityDto University { get; set; } = new();
    public SimpleEntityDto AcademicLevel { get; set; } = new();
    public SimpleEntityDto Semester { get; set; } = new();
    public SimpleEntityDto DocumentType { get; set; } = new();
    public SimpleEntityDto Professor { get; set; } = new();
}
