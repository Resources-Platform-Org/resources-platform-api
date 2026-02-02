using Api.Dtos.Universities;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.AdminControllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class UniversitiesController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UniversitiesController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var universities = await _unitOfWork.Universities.GetAllAsync(null);
            return SuccessResponse
            (
                _mapper.Map<IEnumerable<UniversityResponseDto>>(universities),
                "Fetched universities successfully."
            );
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var university = await _unitOfWork.Universities.GetAsync(x => x.Id == id, true);
            if (university == null) return ErrorResponse("University not found.", 404);

            return SuccessResponse
            (
                _mapper.Map<UniversityResponseDto>(university),
                "Fetched university successfully."
            );
        }
        [HttpPost]
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

        [HttpPut("{id}")]
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var university = await _unitOfWork.Universities.FindAsync(id);
            if (university == null)
                return ErrorResponse("University not found.", 404);

            await _unitOfWork.Universities.DeleteAsync(x => x.Id == id);
            await _unitOfWork.SaveChangesAsync();

            return SuccessResponse(university, "University deleted successfully.");
        }
    }
}
