using Core.Entities;

interface IUserRepository
{
    Task<User?> GetUserByEmailAsync(string email);
    Task GetUserRoleAsync(User user);
    Task IsUsernameOrEmailTakenAsync(string username, string email);
}