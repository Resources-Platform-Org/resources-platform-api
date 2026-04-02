using Bogus;
using Core.Entities;
using Core.Enums;
using System.Collections.Generic;

namespace Infrastructure.Data.Seeding;

public static class ResourcesSeeder
{
    public static List<Resource> GetResources(List<int> documentTypeIds, List<int> courseIds, List<int> userIds, int count = 20)
    {
        var faker = new Faker<Resource>()
            .RuleFor(r => r.Name, f => f.Commerce.ProductName())
            .RuleFor(r => r.Path, f => f.Internet.Url())
            .RuleFor(r => r.StoredFileName, f => f.System.FileName())
            .RuleFor(r => r.DownloadsCount, f => f.Random.Number(0, 1000))
            .RuleFor(r => r.IsApproved, f => f.Random.Bool())
            .RuleFor(r => r.Extension, f => f.PickRandom<enExtension>())
            .RuleFor(r => r.DocumentTypeId, f => f.PickRandom(documentTypeIds))
            .RuleFor(r => r.CourseId, f => f.PickRandom(courseIds))
            .RuleFor(r => r.UploaderId, f => f.PickRandom(userIds))
            .RuleFor(r => r.CreatedAt, f => f.Date.Past(2).ToUniversalTime())
            .RuleFor(r => r.IsDeleted, f => false)
            .RuleFor(r => r.DeletedAt, f => null)
            .RuleFor(r => r.UpdatedAt, f => null);

        return faker.Generate(count);
    }
}
