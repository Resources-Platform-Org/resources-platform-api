using System.ComponentModel.DataAnnotations;

namespace Api.Dtos.Universities
{
    public class UniversityResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
