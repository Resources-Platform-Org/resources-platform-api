using Api.Dtos.Universities;
using Core.Entities;
using Core.Interfaces;
using Core.Models.Majors;
using Core.Models.Universities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.AdminControllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class UniversitiesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public UniversitiesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var universities = await _unitOfWork.Universities.GetAllAsync(null);
            var result = universities.Select(u => new UniversityResponseDto
            {
                UniversityID = u.UniversityID,
                UniversityName = u.UniversityName
            });
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var university = await _unitOfWork.Universities.FindAsync(id);
            if (university == null) return NotFound();

            var result = new UniversityResponseDto
            {
                UniversityID = university.UniversityID,
                UniversityName = university.UniversityName
            };
            return Ok(result);
        }

        [HttpGet("{id}/majors")]
        public async Task<IActionResult> GetMajors(int id)
        {
            var university = await _unitOfWork.Universities.FindAsync(id);
            if (university == null)
                return NotFound("University not found.");

            var majors = await _unitOfWork.Universities.GetMajorsAsync(id);
            
            var result = majors.Select(m => new MajorResponseDto
            {
                MajorID = m.MajorID,
                MajorName = m.MajorName,
                UniversityID = m.UniversityID,
                UniversityName = university.UniversityName
            });
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUniversityDto dto)
        {
            if (await _unitOfWork.Universities.IsNameTakenAsync(dto.UniversityName))
                return BadRequest("University name is already taken.");

            var university = new University
            {
                UniversityName = dto.UniversityName
            };

            await _unitOfWork.Universities.AddAsync(university);
            await _unitOfWork.SaveChangesAsync();

            var result = new UniversityResponseDto
            {
                UniversityID = university.UniversityID,
                UniversityName = university.UniversityName
            };

            return CreatedAtAction(nameof(GetById), new { id = university.UniversityID }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateUniversityDto dto)
        {
            var university = await _unitOfWork.Universities.FindAsync(id);
            if (university == null) return NotFound();

            if (university.UniversityName != dto.UniversityName && 
                await _unitOfWork.Universities.IsNameTakenAsync(dto.UniversityName))
            {
                return BadRequest("University name is already taken.");
            }

            university.UniversityName = dto.UniversityName;
            
            await _unitOfWork.Universities.Update(university);
            await _unitOfWork.SaveChangesAsync();

            var result = new UniversityResponseDto
            {
                UniversityID = university.UniversityID,
                UniversityName = university.UniversityName
            };

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var university = await _unitOfWork.Universities.FindAsync(id);
            if (university == null) return NotFound();

            // Check if university has majors or other dependencies if necessary
            var majors = await _unitOfWork.Universities.GetMajorsAsync(id);
            if (majors.Any())
                return BadRequest("Cannot delete unversity Because it has associated majors , please delete them first.");
            
            await _unitOfWork.Universities.DeleteAsync(u => u.UniversityID == id);
            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }
    }
}
