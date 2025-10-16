using Core.Entities;

public interface IDocumentTypeRepository
{
    Task<Dictionary<enFileType,int>> CountFileByTypeForCourseAsync(int courseId);
}