using Bogus;
using Core.Entities;
using System.Collections.Generic;

namespace Infrastructure.Data.Seeding;

public static class MajorsSeeder
{
    public static List<Major> GetMajors(List<int> universityIds, int count = 20)
    {
        var faker = new Faker<Major>()
            .RuleFor(m => m.Name, f => f.Name.JobArea() + " Engineering")
            .RuleFor(m => m.UniversityId, f => f.PickRandom(universityIds))
            .RuleFor(m => m.CreatedAt, f => f.Date.Past(2).ToUniversalTime())
            .RuleFor(m => m.UpdatedAt, f => null);

        return faker.Generate(count);
    }
}
