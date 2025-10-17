using Core.Entities;

public interface IUniversityRepository : IGenericRepository<University>
{
    Task<IEnumerable<Major>> GetMajorsAsync(int universityId);
    Task<bool> IsNameTakenAsync(string universityName);
}