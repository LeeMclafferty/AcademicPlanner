using AcademicPlanner.CoreEntities;
using AcademicPlanner.UseCases.Interface;

namespace AcademicPlanner.UseCases
{
    public class AssessmentUseCases : IAssessmentUseCases
    {

        private ISQLiteAssessments _dataRepository;
        public AssessmentUseCases(ISQLiteAssessments repository)
        {
            _dataRepository = repository;
        }

        public async Task ExecuteAddAsync(Assessment assessment)
        {
            await _dataRepository.AddAsync(assessment);
        }

        public async Task<List<Assessment>> ExecuteGetAsync()
        {
            return await _dataRepository.GetAllAsync();
        }

        public async Task<List<Assessment>> ExecuteGetByCourseIdAsync(int courseId)
        {
            return await _dataRepository.GetByCourseId(courseId);
        }

        public async Task<Assessment> ExecuteGetByIdAsync(int assessmentId)
        {
            return await _dataRepository.GetByIdAsync(assessmentId);
        }

        public async Task ExecuteRemoveAsync(Assessment assessment)
        {
            await _dataRepository.DeleteAsync(assessment.assessmentId);
        }

        public async Task ExecuteUpdateAsync(Assessment assessment)
        {
            await _dataRepository.UpdateAsync(assessment.assessmentId, assessment);
        }
    }
}
