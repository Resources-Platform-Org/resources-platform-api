using Core.Entities;
using Core.Interfaces;

namespace Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITokenService _tokenService;
    public AuthService(IUnitOfWork unitOfWork, ITokenService tokenService)
    {
        _tokenService = tokenService;
        _unitOfWork = unitOfWork;
    }
    public async Task<(bool Success, string Message, User? user, string? Token)> LoginAsync(string email, string password)
    {
        var user = await _unitOfWork.Users.GetAsync(u => u.Email == email);
        if (user == null)
            return (false, "Invalid credentials", null, null);

        bool isPasswordValid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
        if (!isPasswordValid)
            return (false, "Invalid credentials", null, null);

        var token = _tokenService.GenerateToken(user);
        return (true, "Login Successfuly", user, token);
    }

    public async Task<(bool Success, string Message, User? user)> RegisterAsync(string name, string email, string password)
    {
        if (await _unitOfWork.Users.IsEmailTakenAsync(email))
            return (false, "Email is already taken", null);

        var PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
        var user = new User
        {
            Name = name,
            Email = email,
            PasswordHash = PasswordHash,
            Role = enRoles.Admin // Temporarily Admin for testing
        };

        await _unitOfWork.Users.AddAsync(user);
        await _unitOfWork.SaveChangesAsync();

        return (true, "User registered successfully", user);
    }

    public async Task<(bool IsSuccess, string Message)> ChangePasswordAsync(int userId, string currentPassword, string newPassword)
    {
        var user = await _unitOfWork.Users.GetAsync(x => x.Id == userId);
        if (user == null)
            return (false, "user not found");

        bool isCurrentPasswordValid = BCrypt.Net.BCrypt.Verify(currentPassword, user.PasswordHash);
        if (!isCurrentPasswordValid)
            return (false, "You'r Password Invalid");

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
        _unitOfWork.Users.Update(user);
        await _unitOfWork.SaveChangesAsync();
        return (true, "Changing Password Done Succesfuly");
    }
}