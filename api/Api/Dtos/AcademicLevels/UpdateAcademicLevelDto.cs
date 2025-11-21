using System.ComponentModel.DataAnnotations;
using Core.Enums;

namespace Api.Dtos.AcademicLevels
{
    public class UpdateAcademicLevelDto
    {
        [Required]
        public enLevel LevelName { get; set; }
    }
}
