using Api.Contracts;
using Api.Dtos;
using Api.Dtos.Majors;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.AdminControllers
{
    [Route(ApiRoutes.Majors.Controller)]
    [ApiController]
    public class MajorController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public MajorController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        [HttpGet(ApiRoutes.Majors.GetByUniversityId)]
        public async Task<IActionResult> GetAllByUniversityId(int universityId)
        {
            var majors = await _unitOfWork.Majors.GetAllAsync(x => x.UniversityId == universityId);
            return SuccessResponse
            (
                _mapper.Map<IEnumerable<MajorResponseDto>>(majors),
                "Fetched majors successfully."
            );
        }
        [HttpGet(ApiRoutes.Majors.GetPaged)]
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
        [HttpGet(ApiRoutes.Majors.GetById)]
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

        [HttpPost(ApiRoutes.Majors.Create)]
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
        [HttpPut(ApiRoutes.Majors.Update)]
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

        [HttpDelete(ApiRoutes.Majors.Delete)]
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
