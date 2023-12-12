using AcademicPlanner.CoreEntities;

namespace AcademicPlanner.UseCases.Interface
{
    public interface ISQLiteRepository
    {
        Task AddTermAsync(Term term);
        Task DeleteTermAsync(int termId);
        Task UpdateTermAsync(int termId, Term term);
        Task<Term> GetTermByIdAsync(int termId);
        Task<List<Term>> GetTermsAsync();
        Task InitializeDefaultTermsAsync();
    }
}
