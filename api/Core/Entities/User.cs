namespace Core.Entities;
public class User
{
    public int UserID { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public enRoles Role { get; set; } = enRoles.User ;
    // Navigation Properties
    public ICollection<Entities.File> UploadedFiles { get; set; } = new List<Entities.File>();
}
