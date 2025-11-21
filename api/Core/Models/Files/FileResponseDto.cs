using System;

namespace Core.Models.Files
{
    public class FileResponseDto
    {
        public int FileID { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string FileType { get; set; } = string.Empty;
        public DateOnly UploadDate { get; set; }
        public string UploaderName { get; set; } = string.Empty;
        public string CourseName { get; set; } = string.Empty;
        public string ProfessorName { get; set; } = string.Empty;
        public string DocumentTypeName { get; set; } = string.Empty;
        public int CourseID { get; set; }
        public int ProfessorID { get; set; }
        public int DocumentTypeID { get; set; }
        public int UploaderID { get; set; }
    }
}
