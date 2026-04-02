using System.ComponentModel.DataAnnotations;

namespace Api.Dtos.Majors
{
    public class MajorResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int UniversityId { get; set; }
        public string UniversityName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
