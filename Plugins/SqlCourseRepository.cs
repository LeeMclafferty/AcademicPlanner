using AcademicPlanner.CoreEntities;
using AcademicPlanner.UseCases.Interface;
using SQLite;

namespace DataStore.SQLite
{
    public class SqlCourseRepository : ISQLiteCourses
    {

        private SQLiteAsyncConnection database;

        public SqlCourseRepository() 
        {
            // Create db if one does not exist.
            this.database = new SQLiteAsyncConnection(Constants.DataPath);
            this.database.CreateTableAsync<Course>();
        }

        public async Task AddCourseAsync(Course course)
        {
            await database.InsertAsync(course);
        }

        public async Task DeleteCourseAsync(int courseId)
        {
            Course course = await GetCourseByIdAsync(courseId);

            if (course != null && course.CourseId == courseId)
            {
                // Fetch associated assessment IDs for the course
                var assessmentIdList = await database.QueryAsync<int>("SELECT assessmentId FROM Assessment WHERE courseId = ?", courseId);

                // Delete assessments
                if (assessmentIdList.Count > 0)
                {
                    await database.ExecuteAsync($"DELETE FROM Assessment WHERE assessmentId IN ({string.Join(",", assessmentIdList)})");
                }

                // Delete the course
                await database.DeleteAsync(course);
            }
        }


        public async Task<List<Course>> GetCourseAsync()
        {
            return await this.database.Table<Course>().ToListAsync();
        }

        public async Task<Course> GetCourseByIdAsync(int courseId)
        {
            return await database.Table<Course>().Where(x => x.CourseId == courseId).FirstOrDefaultAsync();
        }

        public async Task UpdateCourseAsync(int courseId, Course course)
        {
            if (courseId == course.CourseId)
            {
                await database.UpdateAsync(course);
            }
        }

        public async Task<List<Course>> GetCoursesByTermIdAsync(int termId)
        {
            return await database.Table<Course>().Where(x => x.TermId == termId).ToListAsync();
        }
    }
}
