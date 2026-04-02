using Bogus;
using Core.Entities;
using System.Collections.Generic;

namespace Infrastructure.Data.Seeding;

public static class UniversitiesSeeder
{
    public static List<University> GetUniversities(int count = 20)
    {
        var faker = new Faker<University>()
            .RuleFor(u => u.Name, f => f.Company.CompanyName() + " University")
            .RuleFor(u => u.CreatedAt, f => f.Date.Past(2).ToUniversalTime())
            .RuleFor(u => u.UpdatedAt, f => null);

        return faker.Generate(count);
    }
}
