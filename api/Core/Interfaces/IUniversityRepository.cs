using Core.Entities;

interface IUniversityRepository
{
    Task<IEnumerable<Major>> GetMajorsAsync(int universityId);
    Task<bool> IsNameTakenAsync(string universityName);
}