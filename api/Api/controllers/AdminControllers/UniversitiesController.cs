using Api.Contracts;
using Api.Dtos;
using Api.Dtos.Universities;
using Api.Wrappers;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.AdminControllers
{
    [Route(ApiRoutes.Universities.Controller)]
    [ApiController]
    [Authorize(Roles = "Admin")]
    [Produces("application/json")]
    public class UniversitiesController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UniversitiesController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        /// <summary>
        /// Gets all universities (List)
        /// </summary>
        /// <returns>List of universities</returns>
        [HttpGet(ApiRoutes.Universities.GetList)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<UniversityResponseDto>>), 200)]
        public async Task<IActionResult> GetAll()
        {
            var universities = await _unitOfWork.Universities.GetAllAsync(null);
            return SuccessResponse
            (
                _mapper.Map<IEnumerable<UniversityResponseDto>>(universities),
                "Fetched universities successfully."
            );
        }

        /// <summary>
        /// Gets paginated universities
        /// </summary>
        /// <param name="query">Pagination parameters</param>
        /// <returns>Paged universities</returns>
        [HttpGet(ApiRoutes.Universities.GetPaged)]
        [ProducesResponseType(typeof(PagedResponse<UniversityResponseDto>), 200)]
        public async Task<IActionResult> GetPaged([FromQuery] PaginationQuery query)
        {
            var pagedUniversities = await _unitOfWork.Universities.GetPagedAsync(
                query.PageNumber,
                query.PageSize,
                null,
                false
            );
            return SuccessResponse
            (
                pagedUniversities,
                "Fetched universities successfully."
            );
        }

        /// <summary>
        /// Gets a university by ID
        /// </summary>
        /// <param name="id">University ID</param>
        /// <returns>University details</returns>
        /// <response code="200">University found</response>
        /// <response code="404">University not found</response>
        [HttpGet(ApiRoutes.Universities.GetById)]
        [ProducesResponseType(typeof(ApiResponse<UniversityResponseDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse<string>), 404)]
        public async Task<IActionResult> GetById(int id)
        {
            var university = await _unitOfWork.Universities.GetAsync(x => x.Id == id);
            if (university == null) return ErrorResponse("University not found.", 404);

            return SuccessResponse
            (
                _mapper.Map<UniversityResponseDto>(university),
                "Fetched university successfully."
            );
        }

        /// <summary>
        /// Creates a new university
        /// </summary>
        /// <param name="dto">University details</param>
        /// <returns>Created university</returns>
        [HttpPost(ApiRoutes.Universities.Create)]
        [ProducesResponseType(typeof(ApiResponse<UniversityResponseDto>), 201)]
        public async Task<IActionResult> Create([FromBody] UniversityDto dto)
        {
            var university = _mapper.Map<University>(dto);
            await _unitOfWork.Universities.AddAsync(university);
            await _unitOfWork.SaveChangesAsync();

            return CreatedResponse
            (
                _mapper.Map<UniversityResponseDto>(university),
                "University created successfully."
            );
        }

        /// <summary>
        /// Updates an existing university
        /// </summary>
        /// <param name="id">University ID</param>
        /// <param name="dto">Updated details</param>
        /// <returns>Updated university</returns>
        /// <response code="200">University updated</response>
        /// <response code="404">University not found</response>
        [HttpPut(ApiRoutes.Universities.Update)]
        [ProducesResponseType(typeof(ApiResponse<UniversityResponseDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse<string>), 404)]
        public async Task<IActionResult> Update(int id, [FromBody] UniversityDto dto)
        {
            var university = await _unitOfWork.Universities.FindAsync(id);
            if (university == null)
                return ErrorResponse("University not found.", 404);

            _mapper.Map(dto, university);
            _unitOfWork.Universities.Update(university);
            await _unitOfWork.SaveChangesAsync();

            return SuccessResponse
            (
                _mapper.Map<UniversityResponseDto>(university),
                "University updated successfully."
            );
        }

        /// <summary>
        /// Deletes a university
        /// </summary>
        /// <param name="id">University ID</param>
        /// <response code="200">University deleted</response>
        /// <response code="404">University not found</response>
        [HttpDelete(ApiRoutes.Universities.Delete)]
        [ProducesResponseType(typeof(ApiResponse<string>), 200)]
        [ProducesResponseType(typeof(ApiResponse<string>), 404)]
        public async Task<IActionResult> Delete(int id)
        {
            var university = await _unitOfWork.Universities.FindAsync(id);
            if (university == null)
                return ErrorResponse("University not found.", 404);

            await _unitOfWork.Universities.DeleteAsync(x => x.Id == university.Id);

            return SuccessResponse<object>
            (
                null!,
                "University deleted successfully."
            );
        }
    }
}
