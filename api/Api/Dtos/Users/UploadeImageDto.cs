using System.ComponentModel.DataAnnotations;

namespace Api.Dtos.Users;

public class UploadeImageDto
{
    [Required(ErrorMessage = "File is required.")]
    public IFormFile File { get; set; } = null!;
}