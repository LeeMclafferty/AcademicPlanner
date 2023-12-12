using AcademicPlanner.CoreEntities;

namespace AcademicPlanner.UseCases.Interface
{
    public interface ITermUseCases
    {
        Task ExecuteAddAsync(CoreEntities.Term term);
        Task ExecuteRemoveAsync(CoreEntities.Term term);
        Task ExecuteUpdateAsync(Term term);
        Task<Term> ExecuteGetTermByIdAsync(int termId);
        Task<List<Term>> ExecuteGetTermsAsync();
        Task ExecuteInitDatabase();
    }
}