using System.ComponentModel.DataAnnotations;

namespace Api.Dtos.Users;

public class ChangePasswordDto
{
    [Required(ErrorMessage = "Current password is required.")]
    public string CurrentPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "New password is required.")]
    [MinLength(6, ErrorMessage = "New password must be at least 6 characters long.")]
    public string NewPassword { get; set; } = string.Empty;
    [Compare("NewPassword", ErrorMessage = "Confirm password does not match.")]
    public string ConfirmNewPassword { get; set; } = null!;
}