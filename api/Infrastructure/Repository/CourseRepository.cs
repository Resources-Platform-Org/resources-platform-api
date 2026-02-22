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

    public async Task<Course?> GetCourseWithDetailsAsync(int courseId)
    {
        var course = await _context.Courses
            .AsNoTracking()
            .Include(c => c.CourseMajors)
                .ThenInclude(cm => cm.Major)
            .Include(c => c.Professors)
            .Include(c => c.Resources)
            .AsSplitQuery()
            .FirstOrDefaultAsync(c => c.Id == courseId);
        return course;
    }

    public async Task<(IEnumerable<Course> Items, int TotalCount)> GetPagedCoursesAsync(int pageNumber, int pageSize)
    {
        var baseQuery = _context.Courses.AsNoTracking();

        var totalCount = await baseQuery.CountAsync();
        var items = await baseQuery
            .Include(c => c.CourseMajors)
                .ThenInclude(cm => cm.Major)
            .Include(c => c.Professors)
            .AsSplitQuery()
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }
}