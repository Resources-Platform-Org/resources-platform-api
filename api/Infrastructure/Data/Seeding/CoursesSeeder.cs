using Bogus;
using Core.Entities;
using System.Collections.Generic;

namespace Infrastructure.Data.Seeding;

public static class CoursesSeeder
{
    public static List<Course> GetCourses(int count = 20)
    {
        var faker = new Faker<Course>()
            .RuleFor(c => c.Name, f => f.Commerce.Department() + " 101")
            .RuleFor(c => c.Code, f => f.Random.String2(4, "ABCDEFGHIJKLMNOPQRSTUVWXYZ") + f.Random.Number(100, 499).ToString())
            .RuleFor(c => c.CreatedAt, f => f.Date.Past(2).ToUniversalTime())
            .RuleFor(c => c.UpdatedAt, f => null);

        return faker.Generate(count);
    }
}
