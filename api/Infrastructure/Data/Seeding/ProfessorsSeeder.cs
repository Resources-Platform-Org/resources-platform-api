using Bogus;
using Core.Entities;
using System.Collections.Generic;

namespace Infrastructure.Data.Seeding;

public static class ProfessorsSeeder
{
    public static List<Professor> GetProfessors(int count = 20)
    {
        var faker = new Faker<Professor>()
            .RuleFor(p => p.Name, f => f.Name.FullName())
            .RuleFor(p => p.Phone, f => f.Phone.PhoneNumber())
            .RuleFor(p => p.CreatedAt, f => f.Date.Past(2).ToUniversalTime())
            .RuleFor(p => p.UpdatedAt, f => null);

        return faker.Generate(count);
    }
}
