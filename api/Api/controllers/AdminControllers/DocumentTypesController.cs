using Api.Contracts;
using Api.Dtos;
using Api.Dtos.DocumentTypes;
using Api.Wrappers;
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
    [Produces("application/json")]
    public class DocumentTypesController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DocumentTypesController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        /// <summary>
        /// Gets all document types (list)
        /// </summary>
        /// <returns>List of document types</returns>
        [HttpGet(ApiRoutes.DocumentTypes.GetList)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<DocumentTypeResponseDto>>), 200)]
        public async Task<IActionResult> GetAll()
        {
            var DocTypes = await _unitOfWork.DocumentsType.GetAllAsync(null);
            return SuccessResponse(
                _mapper.Map<IEnumerable<DocumentTypeResponseDto>>(DocTypes),
                "Document types retrieved successfully."
            );
        }

        /// <summary>
        /// Gets paginated document types
        /// </summary>
        /// <param name="query">Pagination parameters</param>
        /// <returns>Paged document types</returns>
        [HttpGet(ApiRoutes.DocumentTypes.GetPaged)]
        [ProducesResponseType(typeof(PagedResponse<DocumentTypeResponseDto>), 200)]
        public async Task<IActionResult> GetPaged([FromQuery] PaginationQuery query)
        {
            var pagedDocTypes = await _unitOfWork.DocumentsType.GetPagedAsync(
                query.PageNumber,
                query.PageSize,
                null,
                false
            );

            var response = _mapper.Map<IEnumerable<DocumentTypeResponseDto>>(pagedDocTypes.Items);
            return PagedResponse
            (
                response,
                query.PageNumber,
                query.PageSize,
                pagedDocTypes.TotalCount
            );
        }

        /// <summary>
        /// Gets a document type by ID
        /// </summary>
        /// <param name="id">Document Type ID</param>
        /// <returns>Document type details</returns>
        /// <response code="200">Document type found</response>
        /// <response code="404">Document type not found</response>
        [HttpGet(ApiRoutes.DocumentTypes.GetById)]
        [ProducesResponseType(typeof(ApiResponse<DocumentTypeResponseDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse<string>), 404)]
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

        /// <summary>
        /// Creates a new document type
        /// </summary>
        /// <param name="dto">Document type details</param>
        /// <returns>Created document type</returns>
        [HttpPost(ApiRoutes.DocumentTypes.Create)]
        [ProducesResponseType(typeof(ApiResponse<DocumentTypeResponseDto>), 201)]
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

        /// <summary>
        /// Updates an existing document type
        /// </summary>
        /// <param name="id">Document Type ID</param>
        /// <param name="dto">Updated details</param>
        /// <returns>Updated document type</returns>
        /// <response code="200">Document type updated</response>
        /// <response code="404">Document type not found</response>
        [HttpPut(ApiRoutes.DocumentTypes.Update)]
        [ProducesResponseType(typeof(ApiResponse<DocumentTypeResponseDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse<string>), 404)]
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

        /// <summary>
        /// Deletes a document type
        /// </summary>
        /// <param name="id">Document Type ID</param>
        /// <response code="200">Document type deleted</response>
        /// <response code="404">Document type not found</response>
        [HttpDelete(ApiRoutes.DocumentTypes.Delete)]
        [ProducesResponseType(typeof(ApiResponse<string>), 200)]
        [ProducesResponseType(typeof(ApiResponse<string>), 404)]
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
