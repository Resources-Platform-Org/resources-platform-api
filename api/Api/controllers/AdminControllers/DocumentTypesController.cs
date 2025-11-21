using Api.Dtos.DocumentTypes;
using Core.Entities;
using Core.Interfaces;
using Core.Models.DocumentTypes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.AdminControllers
{
    [Route("api/document-types")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class DocumentTypesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public DocumentTypesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var types = await _unitOfWork.DocumentTypes.GetAllAsync(null);
            var result = types.Select(t => new DocumentTypeResponseDto
            {
                DocumentTypeID = t.DocumentTypeID,
                TypeName = t.TypeName.ToString()
            });
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var type = await _unitOfWork.DocumentTypes.FindAsync(id);
            if (type == null) return NotFound();

            var result = new DocumentTypeResponseDto
            {
                DocumentTypeID = type.DocumentTypeID,
                TypeName = type.TypeName.ToString()
            };
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateDocumentTypeDto dto)
        {
            var type = new DocumentType
            {
                TypeName = dto.TypeName
            };

            await _unitOfWork.DocumentTypes.AddAsync(type);
            await _unitOfWork.SaveChangesAsync();

            var result = new DocumentTypeResponseDto
            {
                DocumentTypeID = type.DocumentTypeID,
                TypeName = type.TypeName.ToString()
            };

            return CreatedAtAction(nameof(GetById), new { id = type.DocumentTypeID }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateDocumentTypeDto dto)
        {
            var type = await _unitOfWork.DocumentTypes.FindAsync(id);
            if (type == null) return NotFound();

            type.TypeName = dto.TypeName;

            await _unitOfWork.DocumentTypes.Update(type);
            await _unitOfWork.SaveChangesAsync();

            var result = new DocumentTypeResponseDto
            {
                DocumentTypeID = type.DocumentTypeID,
                TypeName = type.TypeName.ToString()
            };

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var type = await _unitOfWork.DocumentTypes.FindAsync(id);
            if (type == null) return NotFound();

            await _unitOfWork.DocumentTypes.DeleteAsync(t => t.DocumentTypeID == id);
            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }
    }
}
