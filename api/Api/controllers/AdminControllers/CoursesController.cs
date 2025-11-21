using Api.Dtos.Courses;
using Core.Entities;
using Core.Interfaces;
using Core.Models.Courses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.AdminControllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class CoursesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public CoursesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var courses = await _unitOfWork.Courses.GetAllAsync(null);
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var course = await _unitOfWork.Courses.FindAsync(id);
            if (course == null) return NotFound();

            var result = new CourseResponseDto
            {
                CourseID = course.CourseID,
                CourseName = course.CourseName,
                CourseCode = course.CourseCode,
                MajorID = course.MajorID,
                LevelID = course.LevelID,
                SemesterID = course.SemesterID
            };
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCourseDto dto)
        {
            // Validate Foreign Keys
            if (!await _unitOfWork.MajorUniversities.ExistsAsync(m => m.MajorID == dto.MajorID))
                return BadRequest("Invalid Major ID.");
            
            if (!await _unitOfWork.AcademicLevels.ExistsAsync(l => l.LevelID == dto.LevelID))
                return BadRequest("Invalid Level ID.");

            if (!await _unitOfWork.Semesters.ExistsAsync(s => s.SemesterID == dto.SemesterID))
                return BadRequest("Invalid Semester ID.");

            var course = new Course
            {
                CourseName = dto.CourseName,
                CourseCode = dto.CourseCode,
                MajorID = dto.MajorID,
                LevelID = dto.LevelID,
                SemesterID = dto.SemesterID
            };

            await _unitOfWork.Courses.AddAsync(course);
            await _unitOfWork.SaveChangesAsync();

            var result = new CourseResponseDto
            {
                CourseID = course.CourseID,
                CourseName = course.CourseName,
                CourseCode = course.CourseCode,
                MajorID = course.MajorID,
                LevelID = course.LevelID,
                SemesterID = course.SemesterID
            };

            return CreatedAtAction(nameof(GetById), new { id = course.CourseID }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCourseDto dto)
        {
            var course = await _unitOfWork.Courses.FindAsync(id);
            if (course == null) return NotFound();

            // Validate Foreign Keys if changed
            if (course.MajorID != dto.MajorID && !await _unitOfWork.MajorUniversities.ExistsAsync(m => m.MajorID == dto.MajorID))
                return BadRequest("Invalid Major ID.");

            if (course.LevelID != dto.LevelID && !await _unitOfWork.AcademicLevels.ExistsAsync(l => l.LevelID == dto.LevelID))
                return BadRequest("Invalid Level ID.");

            if (course.SemesterID != dto.SemesterID && !await _unitOfWork.Semesters.ExistsAsync(s => s.SemesterID == dto.SemesterID))
                return BadRequest("Invalid Semester ID.");

            course.CourseName = dto.CourseName;
            course.CourseCode = dto.CourseCode;
            course.MajorID = dto.MajorID;
            course.LevelID = dto.LevelID;
            course.SemesterID = dto.SemesterID;

            await _unitOfWork.Courses.Update(course);
            await _unitOfWork.SaveChangesAsync();

            var result = new CourseResponseDto
            {
                CourseID = course.CourseID,
                CourseName = course.CourseName,
                CourseCode = course.CourseCode,
                MajorID = course.MajorID,
                LevelID = course.LevelID,
                SemesterID = course.SemesterID
            };

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var course = await _unitOfWork.Courses.FindAsync(id);
            if (course == null) return NotFound();

            await _unitOfWork.Courses.DeleteAsync(c => c.CourseID == id);
            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }
    }
}
