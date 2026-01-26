using System;
using Core.Entities;
using Core.Enums;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
namespace Infrastructure.Repository;

public class CourseRepository : GenericRepository<Course>, ICourseRepository
{
    private readonly ApplicationDbContext _context;
    public CourseRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Course>> GetCoursesByLevelAndMajorAsync(int majorId, int levelId)
    {
        var result = await _context.Courses
            .Where(c => c.CourseMajors.Any(cm => cm.MajorId == majorId && cm.Level == (enLevel)levelId))
            .ToListAsync();
        return result;
    }

    public Task<Course?> GetCourseWithDetailsAsync(int courseId)
    {
        var course = _context.Courses
            .Include(c => c.CourseMajors)
                .ThenInclude(cm => cm.Major)
            .Include(c => c.Professors)
            .Include(c => c.Resources)
            .FirstOrDefaultAsync(c => c.Id == courseId);
        return course;
    }
}