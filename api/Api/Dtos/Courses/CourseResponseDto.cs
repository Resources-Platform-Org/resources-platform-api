using System.ComponentModel.DataAnnotations;
using Core.Entities;

namespace Api.Dtos.Courses
{
    public class CourseResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public List<string>? Professors { get; set; } = new List<string>();
        public List<string>? CourseMajors { get; set; } = new List<string>();
        public int? FileCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
