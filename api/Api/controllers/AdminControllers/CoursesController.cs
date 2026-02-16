namespace Api.Controllers.AdminControllers;

using Api.Contracts;
using Api.Controllers;
using Api.Dtos;
using Api.Dtos.Courses;
using Api.Wrappers;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route(ApiRoutes.Courses.Controller)]
[Produces("application/json")]
public class CoursesController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public CoursesController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    /// <summary>
    /// Gets all courses (list)
    /// </summary>
    /// <returns>List of courses</returns>
    [HttpGet(ApiRoutes.Courses.GetList)]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<CourseResponseDto>>), 200)]
    public async Task<IActionResult> GetAll()
    {
        var courses = await _unitOfWork.Courses.GetAllAsync(null);
        return SuccessResponse
        (
            _mapper.Map<IEnumerable<CourseResponseDto>>(courses),
            "Courses retrieved successfully"
        );
    }

    /// <summary>
    /// Gets paginated courses
    /// </summary>
    /// <param name="query">Pagination parameters</param>
    /// <returns>Paged courses</returns>
    [HttpGet(ApiRoutes.Courses.GetPaged)]
    [ProducesResponseType(typeof(PagedResponse<CourseResponseDto>), 200)]
    public async Task<IActionResult> GetPaged([FromQuery] PaginationQuery query)
    {
        var (items, totalCount) = await _unitOfWork.Courses.GetPagedCoursesAsync
        (
            pageNumber: query.PageNumber,
            pageSize: query.PageSize
        );
        return PagedResponse
        (
            _mapper.Map<IEnumerable<CourseResponseDto>>(items),
            query.PageNumber,
            query.PageSize,
            totalCount
        );
    }

    /// <summary>
    /// Gets a course by ID
    /// </summary>
    /// <param name="id">Course ID</param>
    /// <returns>Course details</returns>
    /// <response code="200">Course found</response>
    /// <response code="404">Course not found</response>
    [HttpGet(ApiRoutes.Courses.GetById)]
    [ProducesResponseType(typeof(ApiResponse<CourseResponseDto>), 200)]
    [ProducesResponseType(typeof(ApiResponse<string>), 404)]
    public async Task<IActionResult> GetById(int id)
    {
        var course = await _unitOfWork.Courses.GetCourseWithDetailsAsync(id);
        if (course == null)
            return ErrorResponse("Course not found", 404);

        return SuccessResponse
        (
            _mapper.Map<CourseResponseDto>(course),
            "Course retrieved successfully"
        );
    }

    /// <summary>
    /// Gets courses filtered by major and level
    /// </summary>
    /// <param name="majorId">Major ID</param>
    /// <param name="level">Course Level</param>
    /// <returns>List of filtered courses</returns>
    [HttpGet(ApiRoutes.Courses.GetByFilter)]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<CourseResponseDto>>), 200)]
    public async Task<IActionResult> GetByFilter([FromQuery] int majorId, [FromQuery] int level)
    {
        var courses = await _unitOfWork.Courses.GetCoursesByLevelAndMajorAsync(majorId, level);
        return SuccessResponse
        (
            _mapper.Map<IEnumerable<CourseResponseDto>>(courses),
            "Courses retrieved successfully"
        );
    }

    /// <summary>
    /// Creates a new course
    /// </summary>
    /// <param name="courseDto">Course details</param>
    /// <returns>Created course</returns>
    [HttpPost(ApiRoutes.Courses.Create)]
    [ProducesResponseType(typeof(ApiResponse<CourseResponseDto>), 201)]
    public async Task<IActionResult> Create(CourseDto courseDto)
    {
        var course = _mapper.Map<Course>(courseDto);
        await _unitOfWork.Courses.AddAsync(course);
        await _unitOfWork.SaveChangesAsync();

        return CreatedResponse
        (
            _mapper.Map<CourseResponseDto>(course),
            "Course created successfully"
        );
    }

    /// <summary>
    /// Updates an existing course
    /// </summary>
    /// <param name="id">Course ID</param>
    /// <param name="courseDto">Updated details</param>
    /// <returns>Updated course</returns>
    /// <response code="200">Course updated</response>
    /// <response code="404">Course not found</response>
    [HttpPut(ApiRoutes.Courses.Update)]
    [ProducesResponseType(typeof(ApiResponse<CourseResponseDto>), 200)]
    [ProducesResponseType(typeof(ApiResponse<string>), 404)]
    public async Task<IActionResult> Update(int id, CourseDto courseDto)
    {
        var existingCourse = await _unitOfWork.Courses.FindAsync(id);
        if (existingCourse == null)
            return ErrorResponse("Course not found", 404);

        _mapper.Map(courseDto, existingCourse);
        _unitOfWork.Courses.Update(existingCourse);
        await _unitOfWork.SaveChangesAsync();

        return SuccessResponse
        (
            _mapper.Map<CourseResponseDto>(existingCourse),
            "Course updated successfully"
        );
    }
    [HttpDelete(ApiRoutes.Courses.Delete)]
    public async Task<IActionResult> Delete(int id)
    {
        var existingCourse = await _unitOfWork.Courses.FindAsync(id);
        if (existingCourse == null)
            return ErrorResponse("Course not found", 404);

        _unitOfWork.Courses.Remove(existingCourse);
        await _unitOfWork.SaveChangesAsync();

        return SuccessResponse<object>
        (
            null!,
            "Course deleted successfully"
        );
    }
}