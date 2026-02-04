using Api.Contracts;
using Api.Dtos.Professors;
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
    public class ProfessorsController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProfessorsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet(ApiRoutes.Professors.GetAll)]
        public async Task<IActionResult> GetAll()
        {
            var professors = await _unitOfWork.Professors.GetAllAsync(null);
            return SuccessResponse
            (
                _mapper.Map<IEnumerable<ProfessorResponseDto>>(professors),

                "Professors retrieved successfully."
            );
        }

        [HttpGet(ApiRoutes.Professors.GetById)]
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

        [HttpPost(ApiRoutes.Professors.Create)]
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

        [HttpPut(ApiRoutes.Professors.Update)]
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

        [HttpDelete(ApiRoutes.Professors.Delete)]
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
