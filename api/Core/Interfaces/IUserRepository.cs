using Core.Entities;

public interface IUserRepository : IGenericRepository<User>
{
    Task<User?> GetUserByEmailAsync(string email);
    Task<string?> GetUserRoleAsync(User user);
    Task<bool> IsUsernameOrEmailTakenAsync(string username, string email);
}