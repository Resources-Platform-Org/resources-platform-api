using System.ComponentModel.DataAnnotations;
using Core.Enums;

namespace Api.Dtos.AcademicLevels
{
    public class CreateAcademicLevelDto
    {
        [Required]
        public enLevel LevelName { get; set; }
    }
}
