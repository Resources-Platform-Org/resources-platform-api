using Api.Dtos.AcademicLevels;
using Core.Entities;
using Core.Interfaces;
using Core.Models.AcademicLevels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.AdminControllers
{
    [Route("api/academic-levels")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AcademicLevelsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public AcademicLevelsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var levels = await _unitOfWork.AcademicLevels.GetAllAsync(null);
            var result = levels.Select(l => new AcademicLevelResponseDto
            {
                LevelID = l.LevelID,
                LevelName = l.LevelName.ToString()
            });
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var level = await _unitOfWork.AcademicLevels.FindAsync(id);
            if (level == null) return NotFound();

            var result = new AcademicLevelResponseDto
            {
                LevelID = level.LevelID,
                LevelName = level.LevelName.ToString()
            };
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAcademicLevelDto dto)
        {
            var level = new AcademicLevel
            {
                LevelName = dto.LevelName
            };

            await _unitOfWork.AcademicLevels.AddAsync(level);
            await _unitOfWork.SaveChangesAsync();

            var result = new AcademicLevelResponseDto
            {
                LevelID = level.LevelID,
                LevelName = level.LevelName.ToString()
            };

            return CreatedAtAction(nameof(GetById), new { id = level.LevelID }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateAcademicLevelDto dto)
        {
            var level = await _unitOfWork.AcademicLevels.FindAsync(id);
            if (level == null) return NotFound();

            level.LevelName = dto.LevelName;

            await _unitOfWork.AcademicLevels.Update(level);
            await _unitOfWork.SaveChangesAsync();

            var result = new AcademicLevelResponseDto
            {
                LevelID = level.LevelID,
                LevelName = level.LevelName.ToString()
            };

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var level = await _unitOfWork.AcademicLevels.FindAsync(id);
            if (level == null) return NotFound();

            await _unitOfWork.AcademicLevels.DeleteAsync(l => l.LevelID == id);
            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }
    }
}
