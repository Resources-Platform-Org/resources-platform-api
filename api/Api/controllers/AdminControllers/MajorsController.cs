using Api.Dtos.Majors;
using Core.Entities;
using Core.Interfaces;
using Core.Models.Courses;
using Core.Models.Majors;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.AdminControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MajorsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public MajorsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var majors = await _unitOfWork.MajorUniversities.GetAllAsync(null, "University");
            var result = majors.Select(m => new MajorResponseDto
            {
                MajorID = m.MajorID,
                MajorName = m.MajorName,
                UniversityID = m.UniversityID,
                UniversityName = m.University?.UniversityName ?? ""
            });
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var major = await _unitOfWork.MajorUniversities.GetAsync(m => m.MajorID == id, "University");
            if (major == null) return NotFound();

            var result = new MajorResponseDto
            {
                MajorID = major.MajorID,
                MajorName = major.MajorName,
                UniversityID = major.UniversityID,
                UniversityName = major.University?.UniversityName ?? ""
            };
            return Ok(result);
        }

        [HttpGet("{id}/courses")]
        public async Task<IActionResult> GetCourses(int id, [FromQuery] int? semesterId, [FromQuery] int? levelId)
        {
            if (!await _unitOfWork.MajorUniversities.ExistsAsync(m => m.MajorID == id))
                return NotFound("Major not found.");

            var courses = await _unitOfWork.MajorUniversities.GetCoursesAsync(id, semesterId, levelId);
            var result = courses.Select(c => new CourseResponseDto
            {
                CourseID = c.CourseID,
                CourseName = c.CourseName,
                CourseCode = c.CourseCode,
                MajorID = c.MajorID,
                LevelID = c.LevelID,
                SemesterID = c.SemesterID
            });
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateMajorDto dto)
        {
            if (await _unitOfWork.MajorUniversities.IsNameTakenAsync(dto.MajorName))
                return BadRequest("Major name is already taken.");

            if (!await _unitOfWork.Universities.ExistsAsync(u => u.UniversityID == dto.UniversityID))
                return BadRequest("Invalid University ID.");

            var major = new Major
            {
                MajorName = dto.MajorName,
                UniversityID = dto.UniversityID
            };

            await _unitOfWork.MajorUniversities.AddAsync(major);
            await _unitOfWork.SaveChangesAsync();

            // Fetch again to get University name if needed, or just return basic info
            // For simplicity, we return what we have.
            var result = new MajorResponseDto
            {
                MajorID = major.MajorID,
                MajorName = major.MajorName,
                UniversityID = major.UniversityID,
                // UniversityName is not loaded here, but we could fetch it if strictly required.
                // Given the pattern, we'll leave it empty or fetch the university to populate it.
                UniversityName = "" 
            };

            // Ideally we might want to load the university name to return a complete DTO
            var university = await _unitOfWork.Universities.FindAsync(dto.UniversityID);
            if (university != null) result.UniversityName = university.UniversityName;

            return CreatedAtAction(nameof(GetById), new { id = major.MajorID }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateMajorDto dto)
        {
            var major = await _unitOfWork.MajorUniversities.FindAsync(id);
            if (major == null) return NotFound();

            if (major.MajorName != dto.MajorName && 
                await _unitOfWork.MajorUniversities.IsNameTakenAsync(dto.MajorName))
            {
                return BadRequest("Major name is already taken.");
            }

            if (major.UniversityID != dto.UniversityID && 
                !await _unitOfWork.Universities.ExistsAsync(u => u.UniversityID == dto.UniversityID))
            {
                return BadRequest("Invalid University ID.");
            }

            major.MajorName = dto.MajorName;
            major.UniversityID = dto.UniversityID;
            
            await _unitOfWork.MajorUniversities.Update(major);
            await _unitOfWork.SaveChangesAsync();

            var result = new MajorResponseDto
            {
                MajorID = major.MajorID,
                MajorName = major.MajorName,
                UniversityID = major.UniversityID,
                UniversityName = "" // Populate if needed
            };
            
            var university = await _unitOfWork.Universities.FindAsync(dto.UniversityID);
            if (university != null) result.UniversityName = university.UniversityName;

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var major = await _unitOfWork.MajorUniversities.FindAsync(id);
            if (major == null) return NotFound();

            await _unitOfWork.MajorUniversities.DeleteAsync(m => m.MajorID == id);
            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }
    }
}
