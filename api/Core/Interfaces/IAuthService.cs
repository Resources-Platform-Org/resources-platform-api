using Core.Entities;

namespace Core.Interfaces;

public interface IAuthService
{
    Task<(bool Success, string Message, User? user, string? Token)> LoginAsync(string email, string password);
    Task<(bool Success, string Message, User? user)> RegisterAsync(string name, string email, string password);
}