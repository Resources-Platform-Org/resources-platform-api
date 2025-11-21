using System.ComponentModel.DataAnnotations;

namespace Api.Dtos.Majors
{
    public class CreateMajorDto
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string MajorName { get; set; } = string.Empty;

        [Required]
        public int UniversityID { get; set; }
    }
}
