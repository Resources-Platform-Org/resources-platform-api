namespace Core.Entities;
public class User
{
    public int UserID { get; set; }
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public enRole Role { get; set; } = enRole.User ;
    // Navigation Properties
    public ICollection<File> Files { get; set; } = new List<File>();
}
