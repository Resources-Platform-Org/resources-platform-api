using System.ComponentModel.DataAnnotations;

namespace Api.Dtos.Professors
{
    public class UpdateProfessorDto
    {
        [Required]
        [StringLength(100)]
        public string ProfessorName { get; set; } = string.Empty;
    }
}
