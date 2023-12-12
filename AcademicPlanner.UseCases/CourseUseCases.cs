using AcademicPlanner.CoreEntities;
using AcademicPlanner.UseCases.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicPlanner.UseCases
{
    public class CourseUseCases : ICourseUseCases
    {

        private ISQLiteCourses _dataRepository;
        public CourseUseCases(ISQLiteCourses repository)
        {
            _dataRepository = repository;
        }

        public async Task ExecuteAddAsync(Course course)
        {
            await _dataRepository.AddCourseAsync(course);
        }

        public async Task ExecuteRemoveAsync(Course course)
        {
            await _dataRepository.DeleteCourseAsync(course.CourseId);
        }

        public async Task ExecuteUpdateAsync(Course course)
        {
            await _dataRepository.UpdateCourseAsync(course.CourseId, course);
        }

        public async Task<Course> ExecuteGetCourseByIdAsync(int courseId)
        {
            return await _dataRepository.GetCourseByIdAsync(courseId);
        }

        public async Task<List<Course>> ExecuteGetCoursesAsync()
        {
            var courses = await _dataRepository.GetCourseAsync();
            return courses;
        }

        public async Task<List<Course>> ExecuteGetCoursesByTermIdAsync(int termId)
        {
            return await _dataRepository.GetCoursesByTermIdAsync(termId);
        }
    }
}
