using System.ComponentModel.DataAnnotations;

namespace Api.Dtos.Majors
{
    public class MajorDto
    {

        public string Name { get; set; } = string.Empty;
        public int UniversityId { get; set; }
    }
}
