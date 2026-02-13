using System.Diagnostics;
using System.Linq.Expressions;
using Api.Contracts;
using Api.Dtos;
using Api.Dtos.Resource;
using Api.Dtos.Resources;
using AutoMapper;
using Core.Entities;
using Core.Enums;
using Core.Interfaces;
using Core.Setting;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route(ApiRoutes.Resources.Controller)]
public class ResourceController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IFileService _fileService;
    private readonly FileSetting _fileSettings;
    public ResourceController(IUnitOfWork unitOfWork, IMapper mapper, IFileService fileService, FileSetting fileSettings)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _fileService = fileService;
        _fileSettings = fileSettings;
    }
    [HttpGet(ApiRoutes.Resources.GetPaged)]
    public async Task<IActionResult> GetPaged([FromQuery] PaginationQuery query, [FromQuery] int? courseId)
    {
        Expression<Func<Resource, bool>>? filter = null;
        if (courseId.HasValue)
            filter = x => x.CourseId == courseId;

        var result = await _unitOfWork.Resources.GetPagedAsync(
            query.PageNumber,
            query.PageSize,
            filter,
            false,
            "Course", "DocumentType", "Uploader"
        );
        var response = _mapper.Map<IEnumerable<ResourceResponseDto>>(result);
        foreach (var item in response)
        {
            item.DownloadUrl = $"{Request.Scheme}://{Request.Host}/{ApiRoutes.Resources.Controller}/download/{item.Id}";
        }
        return PagedResponse(response, query.PageNumber, query.PageSize, result.TotalCount);
    }
    [HttpPost(ApiRoutes.Resources.Create)]
    public async Task<IActionResult> Create([FromBody] CreateResourceDto dto)
    {
        long maxBytes = _fileSettings.MaxFileSizeInMB * 1024 * 1024;
        if (dto.File.Length > maxBytes)
            return ErrorResponse($"File size should not exceed {_fileSettings.MaxFileSizeInMB} MB", 400);

        var fileExt = Path.GetExtension(dto.File.FileName).ToLower();
        var extensionEnum = GetExtensionEnum(fileExt);

        if (!await _unitOfWork.Courses.ExistsAsync(e => e.Id == dto.CourseId))
            return ErrorResponse("Course not found", 404);

        string folderName = _fileSettings.UploaderFolder;
        string storedFileName = await _fileService.UploadFile(dto.File, folderName);

        var resource = _mapper.Map<Resource>(dto);

        // now we add feild that we ignore them in the mapping
        resource.StoredFileName = storedFileName;
        resource.Path = folderName;
        resource.Extension = extensionEnum;
        resource.DownloadsCount = 0;
        resource.IsApproved = true;

        await _unitOfWork.Resources.AddAsync(resource);
        await _unitOfWork.SaveChangesAsync();
        return CreatedResponse(
            _mapper.Map<ResourceResponseDto>(resource),
            ApiRoutes.Resources.GetById.Replace("{id}", resource.Id.ToString())
        );
    }
    [HttpGet(ApiRoutes.Resources.Download)]
    public async Task<IActionResult> Download(int id)
    {
        var resource = await _unitOfWork.Resources.GetAsync(f => f.Id == id);
        if (resource == null)
            return ErrorResponse("Resource not found", 404);

        var filePath = _fileService.GetFilePath(resource.StoredFileName, resource.Path);
        if (!System.IO.File.Exists(filePath))
            return ErrorResponse("File not found on server", 404);

        // some buesness logic 
        resource.DownloadsCount++;
        _unitOfWork.Resources.Update(resource);
        await _unitOfWork.SaveChangesAsync();

        // Write a file :-
        var memory = new MemoryStream();
        using (var stream = new FileStream(filePath, FileMode.Open))
        {
            await stream.CopyToAsync(memory);
        }
        memory.Position = 0;
        string contentType = GetMinmeType(resource.Extension);
        return File(memory, contentType, resource.Name + Path.GetExtension(resource.StoredFileName));
    }

    [HttpDelete(ApiRoutes.Resources.Delete)]
    public async Task<IActionResult> Delete(int id)
    {
        var resource = await _unitOfWork.Resources.GetAsync(f => f.Id == id);
        if (resource == null)
            return ErrorResponse("Resource not found", 404);

        _fileService.DeleteFile(resource.StoredFileName, resource.Path);
        await _unitOfWork.Resources.DeleteAsync(r => r.Id == id);
        return SuccessResponse("Resource deleted successfully");
    }

    private string GetMinmeType(enExtension extension)
    {
        return extension switch
        {
            enExtension.PDF => "application/pdf",
            enExtension.DOCX => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
            enExtension.XLSX => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            enExtension.PPTX => "application/vnd.openxmlformats-officedocument.presentationml.presentation",
            enExtension.TXT => "text/plain",
            enExtension.ZIP => "application/zip",
            enExtension.RAR => "application/x-rar-compressed",
            enExtension.JPG => "image/jpeg",
            enExtension.PNG => "image/png",
            enExtension.GIF => "image/gif",
            _ => "application/octet-stream"
        };
    }

    private enExtension GetExtensionEnum(string extention)
    {
        return extention switch
        {
            ".pdf" => enExtension.PDF,
            ".docx" => enExtension.DOCX,
            ".xlsx" => enExtension.XLSX,
            ".pptx" => enExtension.PPTX,
            ".txt" => enExtension.TXT,
            ".zip" => enExtension.ZIP,
            ".rar" => enExtension.RAR,
            ".jpg" => enExtension.JPG,
            ".jpeg" => enExtension.JPG,
            ".png" => enExtension.PNG,
            ".gif" => enExtension.GIF,
            _ => throw new Exception("Unsupported file type")
        };
    }
}