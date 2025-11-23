using Api.Dtos.Files;
using Api.Services.FileServices;
using Api.Services.Storage;
using Core.Models.Files;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Linq;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FilesController : ControllerBase
    {
        private readonly IFileService _fileService;
        private readonly IStorageService _storageService;

        public FilesController(IFileService fileService, IStorageService storageService)
        {
            _fileService = fileService;
            _storageService = storageService;
        }

        [HttpPost("upload")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Upload([FromForm] CreateFileDto dto)
        {
            if (dto.File == null) return BadRequest("No file uploaded.");

            string savedFileName;
            try
            {
                savedFileName = await _storageService.SaveFileAsync(dto.File);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }

            try
            {
                var metadata = await _fileService.CreateMetadataAsync(dto, savedFileName);
                var url = _storageService.GetPublicUrl(savedFileName);

                return Ok(new
                {
                    id = metadata.FileID,
                    fileName = savedFileName,
                    url
                });
            }
            catch (Exception ex)
            {
                _storageService.DeleteFile(savedFileName);
                return BadRequest($"Metadata creation failed: {ex.Message}");
            }
        }

        [HttpGet("download/{fileName}")]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> Download(string fileName)
        {
            try
            {
                var fileBytes = await _storageService.GetFileBytesAsync(fileName);
                
                var provider = new FileExtensionContentTypeProvider();
                if (!provider.TryGetContentType(fileName, out var contentType))
                {
                    contentType = "application/octet-stream";
                }

                return File(fileBytes, contentType, fileName);
            }
            catch (FileNotFoundException)
            {
                return NotFound("File not found.");
            }
        }

        [HttpGet("list")]
        [AllowAnonymous]
        public async Task<IActionResult> List([FromQuery] FileFilterDto filter)
        {
            // Use FileService to get metadata from DB
            var (items, totalCount) = await _fileService.FilterAsync(filter);

            // Map to response with URLs
            var result = items.Select(f => new
            {
                f.FileID,
                f.FileName,
                f.FileType,
                f.UploadDate,
                UploaderName = f.Uploader?.Username ?? "Unknown",
                CourseName = f.Course?.CourseName ?? "Unknown",
                ProfessorName = f.Professor?.ProfessorName ?? "Unknown",
                DocumentTypeName = f.DocumentType?.TypeName.ToString() ?? "Unknown",
                Url = _storageService.GetPublicUrl(f.FileName)
            });

            return Ok(new { TotalCount = totalCount, Items = result });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var file = await _fileService.GetByIdAsync(id);
            if (file == null) return NotFound();
            
            // Return metadata + URL
            return Ok(new {
                file.FileID,
                file.FileName,
                file.FileType,
                file.UploadDate,
                UploaderName = file.Uploader?.Username ?? "Unknown",
                CourseName = file.Course?.CourseName ?? "Unknown",
                ProfessorName = file.Professor?.ProfessorName ?? "Unknown",
                DocumentTypeName = file.DocumentType?.TypeName.ToString() ?? "Unknown",
                Url = _storageService.GetPublicUrl(file.FileName)
            });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var fileEntity = await _fileService.GetByIdAsync(id);
            if (fileEntity == null) return NotFound();

            _storageService.DeleteFile(fileEntity.FileName);
            await _fileService.DeleteAsync(id);

            return NoContent();
        }
    }
}
