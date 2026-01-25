using System;
using Core.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
namespace Infrastructure.Repository;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
        return user;
    }

    public async Task<string?> GetUserRoleAsync(User user)
    {
        // Re-fetch role from DB to ensure validity/freshness
        var userRole = await _context.Users
            .Where(u => u.Id == user.Id)
            .Select(u => u.Role)
            .FirstOrDefaultAsync();
            
        // If user not found or role invalid (0), return null or handle appropriately
        if (userRole == 0) return null;

        return userRole.ToString();
    }

    public Task<bool> IsEmailTakenAsync(string email)
    {
        return _context.Users.AnyAsync(u => u.Email == email);
    }
}