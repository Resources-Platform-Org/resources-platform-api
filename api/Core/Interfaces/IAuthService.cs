using Core.Entities;

namespace Core.Interfaces;

public interface IAuthService
{
    Task<(bool IsSuccess, string Message)> ChangePasswordAsync(int userId, string currentPassword, string newPassword);
    Task<(bool Success, string Message, User? user, string? Token)> LoginAsync(string email, string password);
    Task<(bool Success, string Message, User? user)> RegisterAsync(string name, string email, string password);
}