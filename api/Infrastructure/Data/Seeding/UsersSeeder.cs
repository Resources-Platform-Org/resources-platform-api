using Bogus;
using Core.Entities;
using Core.Enums;
using System.Collections.Generic;

namespace Infrastructure.Data.Seeding;

public static class UsersSeeder
{
    public static List<User> GetUsers(int count = 20)
    {
        var faker = new Faker<User>()
            .RuleFor(u => u.Name, f => f.Internet.UserName())
            .RuleFor(u => u.Email, f => f.Internet.Email())
            // Just a dummy hashed password
            .RuleFor(u => u.PasswordHash, f => f.Internet.Password())
            .RuleFor(u => u.ProfilePicture, f => f.Internet.Avatar())
            .RuleFor(u => u.Role, f => f.PickRandom<enRoles>())
            .RuleFor(u => u.CreatedAt, f => f.Date.Past(2).ToUniversalTime())
            .RuleFor(u => u.IsDeleted, f => false)
            .RuleFor(u => u.DeletedAt, f => null)
            .RuleFor(u => u.UpdatedAt, f => null);

        return faker.Generate(count);
    }
}
