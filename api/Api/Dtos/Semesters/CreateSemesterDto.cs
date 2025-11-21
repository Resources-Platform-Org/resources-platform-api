using System.ComponentModel.DataAnnotations;

namespace Api.Dtos.Semesters
{
    public class CreateSemesterDto
    {
        [Required]
        [StringLength(50)]
        public string SemesterName { get; set; } = string.Empty;
    }
}
