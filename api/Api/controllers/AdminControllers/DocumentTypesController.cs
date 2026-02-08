using Api.Contracts;
using Api.Dtos;
using Api.Dtos.DocumentTypes;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.AdminControllers
{
    [Route(ApiRoutes.DocumentTypes.Controller)]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class DocumentTypesController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DocumentTypesController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet(ApiRoutes.DocumentTypes.GetList)]
        public async Task<IActionResult> GetAll()
        {
            var DocTypes = await _unitOfWork.DocumentsType.GetAllAsync(null);
            return SuccessResponse(
                _mapper.Map<IEnumerable<DocumentTypeResponseDto>>(DocTypes),
                "Document types retrieved successfully."
            );
        }
        [HttpGet(ApiRoutes.DocumentTypes.GetPaged)]
        public async Task<IActionResult> GetPaged([FromQuery] PaginationQuery query)
        {
            var pagedDocTypes = await _unitOfWork.DocumentsType.GetPagedAsync(
                query.PageNumber,
                query.PageSize,
                null,
                false
            );
            return SuccessResponse
            (
                pagedDocTypes,
                "Document types retrieved successfully."
            );
        }
        [HttpGet(ApiRoutes.DocumentTypes.GetById)]
        public async Task<IActionResult> GetById(int id)
        {
            var DocType = await _unitOfWork.DocumentsType.FindAsync(id);
            if (DocType == null)
                return ErrorResponse("Document type not found.", 404);

            return SuccessResponse
            (
                _mapper.Map<DocumentTypeResponseDto>(DocType),
                "Document type retrieved successfully."
            );
        }

        [HttpPost(ApiRoutes.DocumentTypes.Create)]
        public async Task<IActionResult> Create([FromBody] DocumentTypeDto dto)
        {
            var docType = _mapper.Map<DocumentType>(dto);
            await _unitOfWork.DocumentsType.AddAsync(docType);
            await _unitOfWork.SaveChangesAsync();

            return CreatedResponse
            (
                _mapper.Map<DocumentTypeResponseDto>(docType),
                "Document type created successfully."
            );
        }

        [HttpPut(ApiRoutes.DocumentTypes.Update)]
        public async Task<IActionResult> Update(int id, [FromBody] DocumentTypeDto dto)
        {
            var docType = await _unitOfWork.DocumentsType.FindAsync(id);
            if (docType == null)
                return ErrorResponse("Document-Type not found", 404);

            _mapper.Map(dto, docType);
            _unitOfWork.DocumentsType.Update(docType);
            await _unitOfWork.SaveChangesAsync();
            return SuccessResponse
            (
                _mapper.Map<DocumentTypeResponseDto>(docType),
                "Document type updated successfully."
            );
        }

        [HttpDelete(ApiRoutes.DocumentTypes.Delete)]
        public async Task<IActionResult> Delete(int id)
        {
            var docType = await _unitOfWork.DocumentsType.FindAsync(id);
            if (docType == null)
                return ErrorResponse("Document-Type not found", 404);

            await _unitOfWork.DocumentsType.DeleteAsync(t => t.Id == id);
            return SuccessResponse<object>
            (
                null!,
                "Document type deleted successfully."
            );
        }
    }
}
