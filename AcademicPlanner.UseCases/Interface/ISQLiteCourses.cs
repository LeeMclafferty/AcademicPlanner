using AcademicPlanner.CoreEntities;

namespace AcademicPlanner.UseCases.Interface
{
    public interface ISQLiteCourses
    {
        Task AddCourseAsync(Course course);
        Task DeleteCourseAsync(int courseId);
        Task UpdateCourseAsync(int courseId, Course course);
        Task<Course> GetCourseByIdAsync(int courseId);
        Task<List<Course>> GetCourseAsync();
        public Task<List<Course>> GetCoursesByTermIdAsync(int termId);
    }
}
