using Api.Dtos.Professors;
using Core.Entities;
using Core.Interfaces;
using Core.Models.Professors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.AdminControllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class ProfessorsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProfessorsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var professors = await _unitOfWork.Professors.GetAllAsync(null);
            var result = professors.Select(p => new ProfessorResponseDto
            {
                ProfessorID = p.ProfessorID,
                ProfessorName = p.ProfessorName
            });
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var professor = await _unitOfWork.Professors.FindAsync(id);
            if (professor == null) return NotFound();

            var result = new ProfessorResponseDto
            {
                ProfessorID = professor.ProfessorID,
                ProfessorName = professor.ProfessorName
            };
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProfessorDto dto)
        {
            var professor = new Professor
            {
                ProfessorName = dto.ProfessorName
            };

            await _unitOfWork.Professors.AddAsync(professor);
            await _unitOfWork.SaveChangesAsync();

            var result = new ProfessorResponseDto
            {
                ProfessorID = professor.ProfessorID,
                ProfessorName = professor.ProfessorName
            };

            return CreatedAtAction(nameof(GetById), new { id = professor.ProfessorID }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateProfessorDto dto)
        {
            var professor = await _unitOfWork.Professors.FindAsync(id);
            if (professor == null) return NotFound();

            professor.ProfessorName = dto.ProfessorName;

            await _unitOfWork.Professors.Update(professor);
            await _unitOfWork.SaveChangesAsync();

            var result = new ProfessorResponseDto
            {
                ProfessorID = professor.ProfessorID,
                ProfessorName = professor.ProfessorName
            };

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var professor = await _unitOfWork.Professors.FindAsync(id);
            if (professor == null) return NotFound();

            await _unitOfWork.Professors.DeleteAsync(p => p.ProfessorID == id);
            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }
    }
}
