using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcademicPlanner.CoreEntities;
using AcademicPlanner.UseCases.Interface;

namespace AcademicPlanner.UseCases
{
    public class TermUseCases : ITermUseCases
    {
        private ISQLiteRepository _dataRepository;
        public TermUseCases(ISQLiteRepository repository)
        {
            _dataRepository = repository;
        }

        public async Task ExecuteAddAsync(Term term)
        {
            await _dataRepository.AddTermAsync(term);
        }

        public async Task<Term> ExecuteGetTermByIdAsync(int termId)
        {
            return await _dataRepository.GetTermByIdAsync(termId);
        }

        public async Task<List<Term>> ExecuteGetTermsAsync()
        {
            var terms = await _dataRepository.GetTermsAsync();
            return terms;
        }

        public async Task ExecuteInitDatabase()
        {
            await _dataRepository.InitializeDefaultTermsAsync();
        }

        public async Task ExecuteRemoveAsync(Term term)
        {
            await _dataRepository.DeleteTermAsync(term.TermId);
        }

        public async Task ExecuteUpdateAsync(Term term)
        {
            await _dataRepository.UpdateTermAsync(term.TermId, term);
        }
    }
}
