using Core.Entities;
using Core.Enums;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    private readonly ApplicationDbContext _context;
    public UserRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email);
    }
    public async Task<string?> GetUserRoleAsync(User user)
    {
        if (user == null) throw new ArgumentNullException(nameof(user));
        
        var role = await _context.Users
            .AsNoTracking()
            .Where(u => u.UserID == user.UserID)
            .Select(r => (enRole?)r.Role) 
            .FirstOrDefaultAsync();
        return role?.ToString() ;
    }
    public async Task<bool> IsUsernameOrEmailTakenAsync(string username, string email)
    {
        var isTaken = await _context.Users
            .AnyAsync(u => username == u.Username || email == u.Email);
        return isTaken;
    }
}