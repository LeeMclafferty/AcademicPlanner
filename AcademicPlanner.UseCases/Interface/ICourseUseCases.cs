using AcademicPlanner.CoreEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicPlanner.UseCases.Interface
{
    public interface ICourseUseCases
    {
        Task ExecuteAddAsync(CoreEntities.Course course);
        Task ExecuteRemoveAsync(CoreEntities.Course course);
        Task ExecuteUpdateAsync(Course course);
        Task<Course> ExecuteGetCourseByIdAsync(int courseId);
        Task<List<Course>> ExecuteGetCoursesAsync();
        public Task<List<Course>> ExecuteGetCoursesByTermIdAsync(int termId);
    }
}
