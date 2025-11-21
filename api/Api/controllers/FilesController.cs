using Api.Dtos.Files;
using Core.Entities;
using Core.Interfaces;
using Core.Models.Files;
using Microsoft.AspNetCore.Mvc;
using Core.Enums;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _environment;

        public FilesController(IUnitOfWork unitOfWork, IWebHostEnvironment environment)
        {
            _unitOfWork = unitOfWork;
            _environment = environment;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] FileFilterDto filter)
        {
            var (items, totalCount) = await _unitOfWork.Files.FilterAsync(
                filter.UniversityId,
                filter.MajorId,
                filter.CourseId,
                filter.DocumentTypeId,
                filter.ProfessorId,
                filter.Search,
                filter.Sort,
                filter.Page,
                filter.PageSize
            );

            var result = items.Select(f => new FileResponseDto
            {
                FileID = f.FileID,
                FileName = f.FileName,
                FileType = f.FileType.ToString(),
                UploadDate = f.UploadDate,
                UploaderName = f.Uploader?.Username ?? "Unknown",
                CourseName = f.Course?.CourseName ?? "Unknown",
                ProfessorName = f.Professor?.ProfessorName ?? "Unknown",
                DocumentTypeName = f.DocumentType?.TypeName.ToString() ?? "Unknown",
                CourseID = f.CourseID,
                ProfessorID = f.ProfessorID,
                DocumentTypeID = f.DocumentTypeID,
                UploaderID = f.UploaderID
            });

            return Ok(new { TotalCount = totalCount, Items = result });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var file = await _unitOfWork.Files.GetAsync(f => f.FileID == id, "Uploader", "Course", "Professor", "DocumentType");
            if (file == null) return NotFound();

            var response = new FileResponseDto
            {
                FileID = file.FileID,
                FileName = file.FileName,
                FileType = file.FileType.ToString(),
                UploadDate = file.UploadDate,
                CourseID = file.CourseID,
                ProfessorID = file.ProfessorID,
                DocumentTypeID = file.DocumentTypeID,
                UploaderID = file.UploaderID,
                UploaderName = file.Uploader?.Username ?? "Unknown",
                CourseName = file.Course?.CourseName ?? "Unknown",
                ProfessorName = file.Professor?.ProfessorName ?? "Unknown",
                DocumentTypeName = file.DocumentType?.TypeName.ToString() ?? "Unknown"
            };
            return Ok(response);
        }

        [HttpGet("{id}/download")]
        public async Task<IActionResult> Download(int id)
        {
            var file = await _unitOfWork.Files.FindAsync(id);
            if (file == null) return NotFound();

            var uploadsFolder = Path.Combine(_environment.WebRootPath ?? _environment.ContentRootPath, "uploads");
            var filePath = Path.Combine(uploadsFolder, file.FileName);

            if (!System.IO.File.Exists(filePath))
                return NotFound("File not found on server.");

            var memory = new MemoryStream();
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;

            return File(memory, "application/octet-stream", file.FileName);
        }

        [HttpGet("recent")]
        public async Task<IActionResult> GetRecent([FromQuery] int count = 10)
        {
            // Reusing FilterAsync to get recent files
            var (items, _) = await _unitOfWork.Files.FilterAsync(
                null, null, null, null, null, null, "newest", 1, count
            );
            
            var result = items.Select(f => new FileResponseDto
            {
                FileID = f.FileID,
                FileName = f.FileName,
                FileType = f.FileType.ToString(),
                UploadDate = f.UploadDate,
                UploaderName = f.Uploader?.Username ?? "Unknown",
                CourseName = f.Course?.CourseName ?? "Unknown",
                ProfessorName = f.Professor?.ProfessorName ?? "Unknown",
                DocumentTypeName = f.DocumentType?.TypeName.ToString() ?? "Unknown",
                CourseID = f.CourseID,
                ProfessorID = f.ProfessorID,
                DocumentTypeID = f.DocumentTypeID,
                UploaderID = f.UploaderID
            });

            return Ok(result);
        }

        [HttpGet("popular")]
        public IActionResult GetPopular()
        {
            // Placeholder as requested
            return Ok(new List<FileResponseDto>());
        }

        [HttpPost]
        public async Task<IActionResult> Upload([FromForm] CreateFileDto dto)
        {
            if (dto.File == null || dto.File.Length == 0)
                return BadRequest("No file uploaded.");

            // Validate related entities exist
            if (!await _unitOfWork.Users.ExistsAsync(u => u.UserID == dto.UploaderId))
                return BadRequest("Invalid Uploader ID.");

            if (!await _unitOfWork.Courses.ExistsAsync(c => c.CourseID == dto.CourseId))
                return BadRequest("Invalid Course ID.");

            if (!await _unitOfWork.Professors.ExistsAsync(p => p.ProfessorID == dto.ProfessorId))
                return BadRequest("Invalid Professor ID.");

            if (!await _unitOfWork.DocumentTypes.ExistsAsync(d => d.DocumentTypeID == dto.DocumentTypeId))
                return BadRequest("Invalid Document Type ID.");

            var uploadsFolder = Path.Combine(_environment.WebRootPath ?? _environment.ContentRootPath, "uploads");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            // Use a unique filename to prevent overwrites
            var uniqueFileName = $"{Guid.NewGuid()}_{dto.File.FileName}";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await dto.File.CopyToAsync(stream);
            }

            var fileEntity = new Core.Entities.File
            {
                FileName = uniqueFileName,
                UploadDate = DateOnly.FromDateTime(DateTime.Now),
                UploaderID = dto.UploaderId,
                CourseID = dto.CourseId,
                ProfessorID = dto.ProfessorId,
                DocumentTypeID = dto.DocumentTypeId
            };

            // Simple extension mapping
            var ext = Path.GetExtension(dto.File.FileName).ToLower();
            fileEntity.FileType = ext switch
            {
                ".pdf" => enFileType.PDF,
                ".doc" => enFileType.DOCX,
                ".docx" => enFileType.DOCX,
                // Add other mappings as needed, defaulting to PDF or a generic type if available
                _ => enFileType.PDF 
            };

            await _unitOfWork.Files.AddAsync(fileEntity);
            await _unitOfWork.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = fileEntity.FileID }, new { fileEntity.FileID, fileEntity.FileName });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var file = await _unitOfWork.Files.FindAsync(id);
            if (file == null) return NotFound();

            var uploadsFolder = Path.Combine(_environment.WebRootPath ?? _environment.ContentRootPath, "uploads");
            var filePath = Path.Combine(uploadsFolder, file.FileName);

            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            await _unitOfWork.Files.DeleteAsync(f => f.FileID == id);
            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }
    }
}
