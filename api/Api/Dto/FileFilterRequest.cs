namespace Api.Dto;
public class FileFilterRequest
{
    public int? UniversityId { get; set; }
    public int? MajorId { get; set; }
    public int? CourseId { get; set; }
    public int? AcademicLevelId { get; set; }
    public int? SemesterId { get; set; }
    public int? DocumentTypeId { get; set; }
    public int? ProfessorId { get; set; }
    public string? Search { get; set; }
    public string? Sort { get; set; } = "newest";
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}
