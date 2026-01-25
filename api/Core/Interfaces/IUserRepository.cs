using Core.Entities;

public interface IUserRepository : IGenericRepository<User>
{
    Task<User?> GetUserByEmailAsync(string email);
    Task<string?> GetUserRoleAsync(User user);
    Task<bool> IsEmailTakenAsync(string username, string email);
}