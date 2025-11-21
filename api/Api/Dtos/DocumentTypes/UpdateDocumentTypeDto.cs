using System.ComponentModel.DataAnnotations;
using Core.Enums;

namespace Api.Dtos.DocumentTypes
{
    public class UpdateDocumentTypeDto
    {
        [Required]
        public enDocument TypeName { get; set; }
    }
}
