using System.ComponentModel.DataAnnotations;

namespace Api.Dtos.Courses
{
    public class CreateCourseDto
    {
        [Required]
        [StringLength(200)]
        public string CourseName { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string CourseCode { get; set; } = string.Empty;

        [Required]
        public int MajorID { get; set; }

        [Required]
        public int LevelID { get; set; }

        [Required]
        public int SemesterID { get; set; }
    }
}
