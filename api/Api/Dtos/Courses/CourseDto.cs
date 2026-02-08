using System.ComponentModel.DataAnnotations;

namespace Api.Dtos.Courses
{
    public class CourseDto
    {
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
    }
}
