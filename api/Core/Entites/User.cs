namespace Core.Entites;
public class User
{
    public int UserID { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public enRole Role { get; set; } = enRole.User ;
    public ICollection<File> UploadedFiles { get; set; } = new List<File>();
}
