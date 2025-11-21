namespace Core.Models.Files
{
    public class FileFilterDto
    {
        public int? UniversityId { get; set; }
        public int? MajorId { get; set; }
        public int? CourseId { get; set; }
        public int? DocumentTypeId { get; set; }
        public int? ProfessorId { get; set; }
        public string? Search { get; set; }
        public string? Sort { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
