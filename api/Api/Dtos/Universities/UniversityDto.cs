using System.ComponentModel.DataAnnotations;

namespace Api.Dtos.Universities
{
    public class UniversityDto
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;
    }
}
