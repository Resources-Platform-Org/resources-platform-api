namespace Api.Controllers.AdminControllers;

using Api.Contracts;
using Api.Controllers;
using Api.Dtos;
using Api.Dtos.Courses;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route(ApiRoutes.Courses.Controller)]
public class CoursesController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public CoursesController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    [HttpGet(ApiRoutes.Courses.GetList)]
    public async Task<IActionResult> GetAll()
    {
        var courses = await _unitOfWork.Courses.GetAllAsync(null);
        return SuccessResponse
        (
            _mapper.Map<IEnumerable<CourseResponseDto>>(courses),
            "Courses retrieved successfully"
        );
    }
    [HttpGet(ApiRoutes.Courses.GetPaged)]
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
    [HttpGet(ApiRoutes.Courses.GetById)]
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
    [HttpGet(ApiRoutes.Courses.GetByFilter)]
    public async Task<IActionResult> GetByFilter([FromQuery] int majorId, [FromQuery] int level)
    {
        var courses = await _unitOfWork.Courses.GetCoursesByLevelAndMajorAsync(majorId, level);
        return SuccessResponse
        (
            _mapper.Map<IEnumerable<CourseResponseDto>>(courses),
            "Courses retrieved successfully"
        );
    }

    [HttpPost(ApiRoutes.Courses.Create)]
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
    [HttpPut(ApiRoutes.Courses.Update)]
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