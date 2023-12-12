using AcademicPlanner.CoreEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicPlanner.UseCases.Interface
{
    public interface IAssessmentUseCases
    {
        Task ExecuteAddAsync(Assessment assessment);
        Task ExecuteRemoveAsync(Assessment assessment);
        Task ExecuteUpdateAsync(Assessment assessment);
        Task<Assessment> ExecuteGetByIdAsync(int assessmentId);
        Task<List<Assessment>> ExecuteGetAsync();
        public Task<List<Assessment>> ExecuteGetByCourseIdAsync(int courseId);
    }
}
