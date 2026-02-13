using System.ComponentModel.DataAnnotations;

namespace Api.Dtos.Users;

public class UpdateProfileDto
{
    [Required(ErrorMessage = "Name is required.")]
    [MaxLength(50, ErrorMessage = "Name cannot exceed 50 characters.")]
    public string? Name { get; set; }
}