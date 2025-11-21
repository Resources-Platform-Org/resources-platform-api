using System.ComponentModel.DataAnnotations;
using Core.Enums;

namespace Api.Dtos.DocumentTypes
{
    public class CreateDocumentTypeDto
    {
        [Required]
        public enDocument TypeName { get; set; }
    }
}
