namespace Api.Dtos.Users;

public class UserProfileDto
{
    public int Id { get; set; }
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Role { get; set; } = null!;
    public string? ProfilePictureUrl { get; set; }
    public DateTime CreatedAt { get; set; }
}