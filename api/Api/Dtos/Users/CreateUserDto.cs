using System.ComponentModel.DataAnnotations;
using Core.Enums;

namespace Api.Dtos.Users
{
    public class CreateUserDto
    {
        [Required]
        [MaxLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(6)]
        public string Password { get; set; } = string.Empty;

        // Optional: Allow setting role during creation? 
        // Usually registration is default User. Admin creation might allow Role.
        // I'll leave it out for basic registration or default to User.
    }
}
