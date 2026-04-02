using Bogus;
using Core.Entities;
using System.Collections.Generic;

namespace Infrastructure.Data.Seeding;

public static class DocumentTypesSeeder
{
    public static List<DocumentType> GetDocumentTypes(int count = 20)
    {
        var faker = new Faker<DocumentType>()
            .RuleFor(dt => dt.Name, f => f.Commerce.ProductMaterial() + " Document")
            .RuleFor(dt => dt.CreatedAt, f => f.Date.Past(2).ToUniversalTime())
            .RuleFor(dt => dt.UpdatedAt, f => null);

        return faker.Generate(count);
    }
}
