using Bogus;
using Core.Entities;
using Core.Enums;
using System.Collections.Generic;

namespace Infrastructure.Data.Seeding;

public static class CourseMajorsSeeder
{
    public static List<CourseMajor> GetCourseMajors(List<int> courseIds, List<int> majorIds, int count = 20)
    {
        var faker = new Faker<CourseMajor>()
            .RuleFor(cm => cm.CourseId, f => f.PickRandom(courseIds))
            .RuleFor(cm => cm.MajorId, f => f.PickRandom(majorIds))
            .RuleFor(cm => cm.Semester, f => f.PickRandom<enSemester>())
            .RuleFor(cm => cm.Level, f => f.PickRandom<enLevel>());

        return faker.Generate(count);
    }
}
