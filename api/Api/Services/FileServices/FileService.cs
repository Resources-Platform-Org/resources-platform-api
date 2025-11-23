using Api.Dtos.Files;
using Core.Entities;
using Core.Enums;
using Core.Interfaces;
using Core.Models.Files;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Api.Services.FileServices
{
    public class FileService : IFileService
    {
        private readonly IUnitOfWork _unitOfWork;

        public FileService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Core.Entities.File> CreateMetadataAsync(CreateFileDto dto, string savedFileName)
        {
            if (!await _unitOfWork.Users.ExistsAsync(u => u.UserID == dto.UploaderId))
                throw new ArgumentException("Invalid Uploader ID.");
            if (!await _unitOfWork.Courses.ExistsAsync(c => c.CourseID == dto.CourseId))
                throw new ArgumentException("Invalid Course ID.");
            if (!await _unitOfWork.Professors.ExistsAsync(p => p.ProfessorID == dto.ProfessorId))
                throw new ArgumentException("Invalid Professor ID.");
            if (!await _unitOfWork.DocumentTypes.ExistsAsync(d => d.DocumentTypeID == dto.DocumentTypeId))
                throw new ArgumentException("Invalid Document Type ID.");

            var fileEntity = new Core.Entities.File
            {
                FileName = savedFileName,
                UploadDate = DateOnly.FromDateTime(DateTime.Now),
                UploaderID = dto.UploaderId,
                CourseID = dto.CourseId,
                ProfessorID = dto.ProfessorId,
                DocumentTypeID = dto.DocumentTypeId
            };

            var ext = Path.GetExtension(savedFileName).ToLowerInvariant();
            fileEntity.FileType = ext switch
            {
                ".pdf" => enFileType.PDF,
                ".doc" => enFileType.DOCX,
                ".docx" => enFileType.DOCX,
                _ => enFileType.PDF 
            };

            await _unitOfWork.Files.AddAsync(fileEntity);
            await _unitOfWork.SaveChangesAsync();

            return await GetByIdAsync(fileEntity.FileID) ?? fileEntity;
        }

        public async Task<Core.Entities.File?> GetByIdAsync(int id)
        {
            return await _unitOfWork.Files.GetAsync(f => f.FileID == id, "Uploader", "Course", "Professor", "DocumentType");
        }

        public async Task<(IEnumerable<Core.Entities.File> Items, int TotalCount)> FilterAsync(FileFilterDto filter)
        {
            return await _unitOfWork.Files.FilterAsync(
                filter.UniversityId,
                filter.MajorId,
                filter.CourseId,
                filter.DocumentTypeId,
                filter.ProfessorId,
                filter.Search,
                filter.Sort,
                filter.Page,
                filter.PageSize
            );
        }

        public async Task<IEnumerable<Core.Entities.File>> GetRecentAsync(int count)
        {
            var (items, _) = await _unitOfWork.Files.FilterAsync(
                null, null, null, null, null, null, "newest", 1, count
            );
            return items;
        }

        public async Task DeleteAsync(int id)
        {
            await _unitOfWork.Files.DeleteAsync(f => f.FileID == id);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
