using Api.Contracts;
using Api.Dtos;
using Api.Dtos.Professors;
using Api.Wrappers;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.AdminControllers
{
    [Route(ApiRoutes.Professors.Controller)]
    [ApiController]
    [Authorize(Roles = "Admin")]
    [Produces("application/json")]
    public class ProfessorsController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProfessorsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        /// <summary>
        /// Gets all professors (list)
        /// </summary>
        /// <returns>List of professors</returns>
        [HttpGet(ApiRoutes.Professors.GetList)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ProfessorResponseDto>>), 200)]
        public async Task<IActionResult> GetAll()
        {
            var professors = await _unitOfWork.Professors.GetAllAsync(null);
            return SuccessResponse
            (
                _mapper.Map<IEnumerable<ProfessorResponseDto>>(professors),

                "Professors retrieved successfully."
            );
        }

        /// <summary>
        /// Gets paginated professors
        /// </summary>
        /// <param name="query">Pagination parameters</param>
        /// <returns>Paged professors</returns>
        [HttpGet(ApiRoutes.Professors.GetPaged)]
        [ProducesResponseType(typeof(PagedResponse<ProfessorResponseDto>), 200)]
        public async Task<IActionResult> GetPaged([FromQuery] PaginationQuery query)
        {
            var pagedProfessors = await _unitOfWork.Professors.GetPagedAsync(
                query.PageNumber,
                query.PageSize,
                null,
                false
            );

            var response = _mapper.Map<IEnumerable<ProfessorResponseDto>>(pagedProfessors.Items);
            return PagedResponse
            (
                response,
                query.PageNumber,
                query.PageSize,
                pagedProfessors.TotalCount
            );
        }

        /// <summary>
        /// Gets a professor by ID
        /// </summary>
        /// <param name="id">Professor ID</param>
        /// <returns>Professor details</returns>
        /// <response code="200">Professor found</response>
        /// <response code="404">Professor not found</response>
        [HttpGet(ApiRoutes.Professors.GetById)]
        [ProducesResponseType(typeof(ApiResponse<ProfessorResponseDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse<string>), 404)]
        public async Task<IActionResult> GetById(int id)
        {
            var professor = await _unitOfWork.Professors.FindAsync(id);
            if (professor == null)
                return ErrorResponse("Professor not found.", 404);

            return SuccessResponse
            (
                _mapper.Map<ProfessorResponseDto>(professor),
                "Professor retrieved successfully."
            );
        }

        /// <summary>
        /// Creates a new professor
        /// </summary>
        /// <param name="dto">Professor details</param>
        /// <returns>Created professor</returns>
        [HttpPost(ApiRoutes.Professors.Create)]
        [ProducesResponseType(typeof(ApiResponse<ProfessorResponseDto>), 201)]
        public async Task<IActionResult> Create([FromBody] ProfessorDto dto)
        {
            var professor = _mapper.Map<Professor>(dto);
            await _unitOfWork.Professors.AddAsync(professor);
            await _unitOfWork.SaveChangesAsync();

            return CreatedResponse
            (
                _mapper.Map<ProfessorResponseDto>(professor),
                "Professor created successfully."
            );
        }

        /// <summary>
        /// Updates an existing professor
        /// </summary>
        /// <param name="id">Professor ID</param>
        /// <param name="dto">Updated details</param>
        /// <returns>Updated professor</returns>
        /// <response code="200">Professor updated</response>
        /// <response code="404">Professor not found</response>
        [HttpPut(ApiRoutes.Professors.Update)]
        [ProducesResponseType(typeof(ApiResponse<ProfessorResponseDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse<string>), 404)]
        public async Task<IActionResult> Update(int id, [FromBody] ProfessorDto dto)
        {
            var professor = await _unitOfWork.Professors.FindAsync(id);
            if (professor == null)
                return ErrorResponse("Professor not found.", 404);

            _mapper.Map(dto, professor);
            _unitOfWork.Professors.Update(professor);
            await _unitOfWork.SaveChangesAsync();

            return SuccessResponse
            (
                _mapper.Map<ProfessorResponseDto>(professor),
                "Professor updated successfully."
            );
        }

        /// <summary>
        /// Deletes a professor
        /// </summary>
        /// <param name="id">Professor ID</param>
        /// <response code="200">Professor deleted</response>
        /// <response code="404">Professor not found</response>
        [HttpDelete(ApiRoutes.Professors.Delete)]
        [ProducesResponseType(typeof(ApiResponse<string>), 200)]
        [ProducesResponseType(typeof(ApiResponse<string>), 404)]
        public async Task<IActionResult> Delete(int id)
        {
            var professor = await _unitOfWork.Professors.FindAsync(id);
            if (professor == null)
                return ErrorResponse("Professor not found.", 404);

            await _unitOfWork.Professors.DeleteAsync(x => x.Id == professor.Id);
            return SuccessResponse<object>
            (
                null!,
                "Professor deleted successfully."
            );
        }
    }
}
