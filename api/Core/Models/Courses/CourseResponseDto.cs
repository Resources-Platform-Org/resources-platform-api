namespace Core.Models.Courses
{
    public class CourseResponseDto
    {
        public int CourseID { get; set; }
        public string CourseName { get; set; } = string.Empty;
        public string CourseCode { get; set; } = string.Empty;
        public int MajorID { get; set; }
        public int LevelID { get; set; }
        public int SemesterID { get; set; }
    }
}
