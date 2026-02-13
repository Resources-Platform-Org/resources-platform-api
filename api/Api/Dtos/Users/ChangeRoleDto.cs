using System.ComponentModel.DataAnnotations;

namespace Api.Dtos.Users;

public class ChangeRoleDto
{
    [Required(ErrorMessage = "New role is required.")]
    public enRoles NewRole { get; set; }
}