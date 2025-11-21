using Api.Dtos.Semesters;
using Core.Entities;
using Core.Interfaces;
using Core.Models.Semesters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.AdminControllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class SemestersController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public SemestersController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var semesters = await _unitOfWork.Semesters.GetAllAsync(null);
            var result = semesters.Select(s => new SemesterResponseDto
            {
                SemesterID = s.SemesterID,
                SemesterName = s.SemesterName
            });
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var semester = await _unitOfWork.Semesters.FindAsync(id);
            if (semester == null) return NotFound();

            var result = new SemesterResponseDto
            {
                SemesterID = semester.SemesterID,
                SemesterName = semester.SemesterName
            };
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateSemesterDto dto)
        {
            var semester = new Semester
            {
                SemesterName = dto.SemesterName
            };

            await _unitOfWork.Semesters.AddAsync(semester);
            await _unitOfWork.SaveChangesAsync();

            var result = new SemesterResponseDto
            {
                SemesterID = semester.SemesterID,
                SemesterName = semester.SemesterName
            };

            return CreatedAtAction(nameof(GetById), new { id = semester.SemesterID }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateSemesterDto dto)
        {
            var semester = await _unitOfWork.Semesters.FindAsync(id);
            if (semester == null) return NotFound();

            semester.SemesterName = dto.SemesterName;

            await _unitOfWork.Semesters.Update(semester);
            await _unitOfWork.SaveChangesAsync();

            var result = new SemesterResponseDto
            {
                SemesterID = semester.SemesterID,
                SemesterName = semester.SemesterName
            };

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var semester = await _unitOfWork.Semesters.FindAsync(id);
            if (semester == null) return NotFound();

            await _unitOfWork.Semesters.DeleteAsync(s => s.SemesterID == id);
            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }
    }
}
