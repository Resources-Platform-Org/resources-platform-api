using Api.Wrappers;
using Microsoft.AspNetCore.Mvc;
namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BaseApiController : ControllerBase
{
    protected IActionResult SuccessResponse<T>(T data, string? message = null)
    {
        return Ok(new ApiResponse<T>(data, message));
    }

    protected IActionResult CreatedResponse<T>(T data, string? message = "Item created successfully")
    {
        return StatusCode(201, new ApiResponse<T>(data, message));
    }

    protected IActionResult ErrorResponse(string message, int statusCode = 400, List<string>? errors = null)
    {
        return StatusCode(statusCode, new ApiResponse<object>(message, errors ?? new List<string>()));
    }

    protected IActionResult PagedResponse<T>(IEnumerable<T> data, int pageNumber, int pageSize, int totalRecords)
    {
        return Ok(new PagedResponse<T>(data, pageNumber, pageSize, totalRecords));
    }
}
