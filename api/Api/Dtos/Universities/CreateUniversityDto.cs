using System.ComponentModel.DataAnnotations;

namespace Api.Dtos.Universities
{
    public class CreateUniversityDto
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string UniversityName { get; set; } = string.Empty;
    }
}
