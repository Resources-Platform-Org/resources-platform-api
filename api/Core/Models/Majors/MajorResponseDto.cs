namespace Core.Models.Majors
{
    public class MajorResponseDto
    {
        public int MajorID { get; set; }
        public string MajorName { get; set; } = string.Empty;
        public int UniversityID { get; set; }
        public string UniversityName { get; set; } = string.Empty;
    }
}
