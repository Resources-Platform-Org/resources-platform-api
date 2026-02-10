namespace Core.Setting;

public class FileSetting
{
    public int MaxFileSizeInMB { get; set; }
    public string AllowedExtensions { get; set; } = string.Empty;
    public string UploaderFolder { get; set; } = string.Empty;
}