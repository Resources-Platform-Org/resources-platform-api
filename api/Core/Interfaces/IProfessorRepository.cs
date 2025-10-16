using Core.Entities;

public interface ProfessorRepository
{
    Task<Professor> GetCoursesAsync(int professorId , int? semesterId);
}