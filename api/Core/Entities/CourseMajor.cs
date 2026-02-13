using Core.Enums;

namespace Core.Entities;

public class CourseMajor
{
    public enSemester Semester { get; set; }
    public enLevel Level { get; set; }

    // Foreign Keys
    public int CourseId { get; set; }
    public int MajorId { get; set; }

    // Navigation Properties
    public Course? Course { get; set; }
    public Major? Major { get; set; }
}
