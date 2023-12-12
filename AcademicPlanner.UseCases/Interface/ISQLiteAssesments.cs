using AcademicPlanner.CoreEntities;

namespace AcademicPlanner.UseCases.Interface
{
    public interface ISQLiteAssessments
    {
        Task AddAsync(Assessment assessment);
        Task DeleteAsync(int assessmentId);
        Task UpdateAsync(int assessmentId, Assessment assessment);
        Task<Assessment> GetByIdAsync(int assessmentId);
        Task<List<Assessment>> GetAllAsync();
        public Task<List<Assessment>> GetByCourseId(int courseId);
    }
}
