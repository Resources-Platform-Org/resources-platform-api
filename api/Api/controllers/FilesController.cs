using Api.Dto;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FilesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<FilesController> _logger;
        private readonly IWebHostEnvironment _env;
        private const int MaxPageSize = 100;

        public FilesController(IUnitOfWork unitOfWork, ILogger<FilesController> logger, IWebHostEnvironment env)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _env = env;
        }

        /// <summary>
        /// List files with optional filters, search and pagination.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetFiles([FromQuery] FileFilterRequest request)
        {
            if (request.Page <= 0) return BadRequest(new { message = "page must be > 0" });
            if (request.PageSize <= 0 || request.PageSize > MaxPageSize) return BadRequest(new { message = $"pageSize must be between 1 and {MaxPageSize}" });

            var (items, total) = await _unitOfWork.Files.FilterAsync(
                request.UniversityId,
                request.MajorId,
                request.CourseId,
                request.AcademicLevelId,
                request.SemesterId,
                request.DocumentTypeId,
                request.ProfessorId,
                request.Search,
                request.Sort,
                request.Page,
                request.PageSize);

            var dtoItems = items.Select(f => MapToDto(f)).ToList();
            var result = new PagedResult<FileDto>
            {
                TotalCount = total,
                Page = request.Page,
                PageSize = request.PageSize,
                Items = dtoItems
            };
            return Ok(result);
        }

        /// <summary>
        /// Get single file by id including related metadata.
        /// </summary>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            // Build include paths manually to avoid nameof(File) ambiguity
            var file = await _unitOfWork.Files.GetAsync(f => f.FileID == id,
                "Course.Major.University",
                "Course.AcademicLevel",
                "Course.Semester",
                "Professor",
                "DocumentType");
            if (file == null) return NotFound(new { message = "File not found" });
            return Ok(MapToDto(file));
        }

        /// <summary>
        /// Download physical file content.
        /// </summary>
        [HttpGet("{id:int}/download")]
        public async Task<IActionResult> Download(int id)
        {
            var file = await _unitOfWork.Files.GetAsync(f => f.FileID == id, "Course");
            if (file == null) return NotFound(new { message = "File not found" });

            var uploadsPath = Path.Combine(_env.WebRootPath ?? Path.Combine(_env.ContentRootPath, "wwwroot"), "uploads");
            var physicalPath = Path.Combine(uploadsPath, file.FileName);
            if (!System.IO.File.Exists(physicalPath))
            {
                _logger.LogWarning("File content missing on disk for FileID {FileId} at {Path}", id, physicalPath);
                return NotFound(new { message = "file content storage not configured or file missing" });
            }

            var stream = System.IO.File.OpenRead(physicalPath);
            var contentType = GetContentType(file.FileName);
            return File(stream, contentType, file.FileName);
        }

        /// <summary>
        /// Get recent uploaded files (optionally for a specific course).
        /// </summary>
        [HttpGet("recent")]
        public async Task<IActionResult> Recent([FromQuery] int? courseId, [FromQuery] int count = 10)
        {
            if (count <= 0 || count > 100) return BadRequest(new { message = "count must be between 1 and 100" });
            IEnumerable<Core.Entities.File> files;
            if (courseId.HasValue)
            {
                files = await _unitOfWork.Files.GetLatestForCourseAsync(courseId.Value);
            }
            else
            {
                var all = await _unitOfWork.Files.GetAllAsync(null,
                    "Course.Major.University",
                    "Course.AcademicLevel",
                    "Course.Semester",
                    "Professor",
                    "DocumentType");
                files = all.OrderByDescending(f => f.UploadDate).Take(count);
            }
            return Ok(files.Take(count).Select(f => MapToDto(f)));
        }

        /// <summary>
        /// Popular files placeholder (not implemented yet).
        /// </summary>
        [HttpGet("popular")]
        public IActionResult Popular()
        {
            return StatusCode(501, new { message = "Not implemented" });
        }

        private static FileDto MapToDto(Core.Entities.File f)
        {
            return new FileDto
            {
                FileId = f.FileID,
                FileName = f.FileName,
                FileType = f.FileType,
                UploadDate = f.UploadDate,
                Course = f.Course != null ? new SimpleEntityDto { Id = f.Course.CourseID, Name = f.Course.CourseName } : new(),
                Major = f.Course?.Major != null ? new SimpleEntityDto { Id = f.Course.Major.MajorID, Name = f.Course.Major.MajorName } : new(),
                University = f.Course?.Major?.University != null ? new SimpleEntityDto { Id = f.Course.Major.University.UniversityID, Name = f.Course.Major.University.UniversityName } : new(),
                AcademicLevel = f.Course?.AcademicLevel != null ? new SimpleEntityDto { Id = f.Course.AcademicLevel.LevelID, Name = f.Course.AcademicLevel.LevelName.ToString() } : new(),
                Semester = f.Course?.Semester != null ? new SimpleEntityDto { Id = f.Course.Semester.SemesterID, Name = f.Course.Semester.SemesterName } : new(),
                DocumentType = f.DocumentType != null ? new SimpleEntityDto { Id = f.DocumentType.DocumentTypeID, Name = f.DocumentType.TypeName.ToString() } : new(),
                Professor = f.Professor != null ? new SimpleEntityDto { Id = f.Professor.ProfessorID, Name = f.Professor.ProfessorName } : new(),
            };
        }

        private static string GetContentType(string fileName)
        {
            var ext = Path.GetExtension(fileName).ToLowerInvariant();
            return ext switch
            {
                ".pdf" => "application/pdf",
                ".doc" => "application/msword",
                ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                ".ppt" => "application/vnd.ms-powerpoint",
                ".pptx" => "application/vnd.openxmlformats-officedocument.presentationml.presentation",
                ".xls" => "application/vnd.ms-excel",
                ".xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                ".txt" => "text/plain",
                ".zip" => "application/zip",
                ".rar" => "application/vnd.rar",
                ".jpg" => "image/jpeg",
                ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                _ => "application/octet-stream"
            };
        }
    }
}
