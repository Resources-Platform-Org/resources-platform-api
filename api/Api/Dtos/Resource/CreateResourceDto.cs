using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Api.Dtos.Resources
{
    public class CreateResourceDto
    {
        [Required(ErrorMessage = "The file Name is required.")]
        [MaxLength(100, ErrorMessage = "The name cannot exceed 100 characters.")]
        public string Name { get; set; } = string.Empty;
        [Required]
        public int CourseId { get; set; }
        [Required]
        public int DocumentTypeId { get; set; }
        [Required]
        public int UploadedById { get; set; }

        [Required(ErrorMessage = "The file is required.")]
        public IFormFile File { get; set; } = null!;
    }
}
