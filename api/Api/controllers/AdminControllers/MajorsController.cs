using Api.Contracts;
using Api.Dtos;
using Api.Dtos.Majors;
using Api.Wrappers;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.AdminControllers
{
    [Route(ApiRoutes.Majors.Controller)]
    [ApiController]
    [Produces("application/json")]
    public class MajorController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public MajorController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        /// <summary>
        /// Gets all majors for a university
        /// </summary>
        /// <param name="universityId">University ID</param>
        /// <returns>List of majors</returns>
        [HttpGet(ApiRoutes.Majors.GetByUniversityId)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<MajorResponseDto>>), 200)]
        public async Task<IActionResult> GetAllByUniversityId(int universityId)
        {
            var majors = await _unitOfWork.Majors.GetAllAsync(x => x.UniversityId == universityId);
            return SuccessResponse
            (
                _mapper.Map<IEnumerable<MajorResponseDto>>(majors),
                "Fetched majors successfully."
            );
        }

        /// <summary>
        /// Gets paginated majors
        /// </summary>
        /// <param name="query">Pagination parameters</param>
        /// <param name="universityId">Optional filter by university</param>
        /// <returns>Paged majors</returns>
        [HttpGet(ApiRoutes.Majors.GetPaged)]
        [ProducesResponseType(typeof(PagedResponse<MajorResponseDto>), 200)]
        public async Task<IActionResult> GetPaged([FromQuery] PaginationQuery query, int? universityId = null)
        {
            var pagedMajors = await _unitOfWork.Majors.GetPagedAsync(
                query.PageNumber,
                query.PageSize,
                universityId.HasValue ? x => x.UniversityId == universityId.Value : null,
                false
            );
            return SuccessResponse
            (
                pagedMajors,
                "Fetched majors successfully."
            );
        }

        /// <summary>
        /// Gets a major by ID
        /// </summary>
        /// <param name="id">Major ID</param>
        /// <returns>Major details</returns>
        /// <response code="200">Major found</response>
        /// <response code="404">Major not found</response>
        [HttpGet(ApiRoutes.Majors.GetById)]
        [ProducesResponseType(typeof(ApiResponse<MajorResponseDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse<string>), 404)]
        public async Task<IActionResult> GetById(int id)
        {
            var major = await _unitOfWork.Majors.GetAsync(x => x.Id == id);
            if (major == null)
                return ErrorResponse("Major not found.", 404);

            return SuccessResponse
            (
                _mapper.Map<MajorResponseDto>(major),
                "Fetched major successfully."
            );
        }

        /// <summary>
        /// Creates a new major
        /// </summary>
        /// <param name="dto">Major details</param>
        /// <returns>Created major</returns>
        [HttpPost(ApiRoutes.Majors.Create)]
        [ProducesResponseType(typeof(ApiResponse<MajorResponseDto>), 201)]
        public async Task<IActionResult> Create([FromBody] MajorDto dto)
        {
            var major = _mapper.Map<Major>(dto);
            await _unitOfWork.Majors.AddAsync(major);
            await _unitOfWork.SaveChangesAsync();

            return CreatedResponse
            (
                _mapper.Map<MajorResponseDto>(major),
                "Major created successfully."
            );
        }

        /// <summary>
        /// Updates an existing major
        /// </summary>
        /// <param name="id">Major ID</param>
        /// <param name="dto">Updated details</param>
        /// <returns>Updated major</returns>
        /// <response code="200">Major updated</response>
        /// <response code="404">Major not found</response>
        [HttpPut(ApiRoutes.Majors.Update)]
        [ProducesResponseType(typeof(ApiResponse<MajorResponseDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse<string>), 404)]
        public async Task<IActionResult> Update(int id, [FromBody] MajorDto dto)
        {
            var major = await _unitOfWork.Majors.FindAsync(id);
            if (major == null)
                return ErrorResponse("Major not found.", 404);

            _mapper.Map(dto, major);
            _unitOfWork.Majors.Update(major);
            await _unitOfWork.SaveChangesAsync();
            return SuccessResponse
            (
                _mapper.Map<MajorResponseDto>(major),
                "Major updated successfully."
            );
        }

        /// <summary>
        /// Deletes a major
        /// </summary>
        /// <param name="id">Major ID</param>
        /// <response code="200">Major deleted</response>
        /// <response code="404">Major not found</response>
        [HttpDelete(ApiRoutes.Majors.Delete)]
        [ProducesResponseType(typeof(ApiResponse<string>), 200)]
        [ProducesResponseType(typeof(ApiResponse<string>), 404)]
        public async Task<IActionResult> Delete(int id)
        {
            var major = await _unitOfWork.Majors.FindAsync(id);
            if (major == null)
                return ErrorResponse("Major not found.", 404);

            await _unitOfWork.Majors.DeleteAsync(x => x.Id == major.Id);

            return SuccessResponse<object>
            (
                null!,
                "Major deleted successfully."
            );
        }
    }
}
