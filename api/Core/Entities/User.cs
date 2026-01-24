namespace Core.Entities;
public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public enRoles Role { get; set; } = enRoles.User ;
    // Navigation Properties
    public ICollection<File> UploadedFiles { get; set; } = new List<File>();
}
