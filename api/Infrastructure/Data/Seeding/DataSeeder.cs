using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Data.Seeding;

public static class DataSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        // 1. Universities
        if (!await context.Universities.AnyAsync())
        {
            var universities = UniversitiesSeeder.GetUniversities(20);
            await context.Universities.AddRangeAsync(universities);
            await context.SaveChangesAsync();
        }

        // 2. Majors
        if (!await context.Majors.AnyAsync())
        {
            var universityIds = await context.Universities.Select(u => u.Id).ToListAsync();
            var majors = MajorsSeeder.GetMajors(universityIds, 20);
            await context.Majors.AddRangeAsync(majors);
            await context.SaveChangesAsync();
        }

        // 3. Courses
        if (!await context.Courses.AnyAsync())
        {
            var courses = CoursesSeeder.GetCourses(20);
            await context.Courses.AddRangeAsync(courses);
            await context.SaveChangesAsync();
        }

        // 4. Professors
        if (!await context.Professors.AnyAsync())
        {
            var professors = ProfessorsSeeder.GetProfessors(20);
            await context.Professors.AddRangeAsync(professors);
            await context.SaveChangesAsync();
        }

        // 5. DocumentTypes
        if (!await context.DocumentTypes.AnyAsync())
        {
            var documentTypes = DocumentTypesSeeder.GetDocumentTypes(15);
            await context.DocumentTypes.AddRangeAsync(documentTypes);
            await context.SaveChangesAsync();
        }

        // 6. Users
        // Turn off IgnoreQueryFilters if IsDeleted is filtered globally, 
        // normally we just use regular AnyAsync unless we need to see deleted users too
        if (!await context.Users.AnyAsync())
        {
            var users = UsersSeeder.GetUsers(20);
            await context.Users.AddRangeAsync(users);
            await context.SaveChangesAsync();
        }

        // 7. CourseMajors
        if (!await context.CourseMajors.AnyAsync())
        {
            var courseIds = await context.Courses.Select(c => c.Id).ToListAsync();
            var majorIds = await context.Majors.Select(m => m.Id).ToListAsync();

            // To avoid duplicate composite keys, let's just make one assignment per dummy combination
            var courseMajors = CourseMajorsSeeder.GetCourseMajors(courseIds, majorIds, 20)
                .GroupBy(cm => new { cm.CourseId, cm.MajorId })
                .Select(g => g.First())
                .ToList();

            await context.CourseMajors.AddRangeAsync(courseMajors);
            await context.SaveChangesAsync();
        }

        // 8. Resources
        if (!await context.Resources.AnyAsync())
        {
            var documentTypeIds = await context.DocumentTypes.Select(d => d.Id).ToListAsync();
            var courseIds = await context.Courses.Select(c => c.Id).ToListAsync();
            // Assuming we have User uploaders
            var userIds = await context.Users.Select(u => u.Id).ToListAsync();

            if (documentTypeIds.Any() && courseIds.Any() && userIds.Any())
            {
                var resources = ResourcesSeeder.GetResources(documentTypeIds, courseIds, userIds, 20);
                await context.Resources.AddRangeAsync(resources);
                await context.SaveChangesAsync();
            }
        }
    }
}
