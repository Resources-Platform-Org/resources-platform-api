using System.ComponentModel.DataAnnotations;

namespace Api.Dtos.Professors
{
    public class ProfessorResponseDto
    {
        public int ID { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
