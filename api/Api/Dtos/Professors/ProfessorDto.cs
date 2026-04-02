using System.ComponentModel.DataAnnotations;

namespace Api.Dtos.Professors
{
    public class ProfessorDto
    {
        public string Name { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
    }
}
