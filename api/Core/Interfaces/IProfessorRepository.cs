using Core.Entities;

public interface IProfessorRepository
{
    Task<Professor> GetCoursesAsync(int professorId , int? semesterId);
}