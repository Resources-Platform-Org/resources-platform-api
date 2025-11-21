using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Api.Dtos.Files
{
    public class CreateFileDto
    {
        [Required]
        public IFormFile File { get; set; } = default!;

        [Required]
        public int CourseId { get; set; }

        [Required]
        public int ProfessorId { get; set; }

        [Required]
        public int DocumentTypeId { get; set; }

        [Required]
        public int UploaderId { get; set; }
    }
}
