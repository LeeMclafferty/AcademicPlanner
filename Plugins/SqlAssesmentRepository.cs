using AcademicPlanner.CoreEntities;
using AcademicPlanner.UseCases.Interface;
using SQLite;

namespace DataStore.SQLite
{
    public class SqlAssessmentRepository : ISQLiteAssessments
    {
        private readonly SQLiteAsyncConnection database;
        public SqlAssessmentRepository() 
        {
            // Create db if one does not exist.
            this.database = new SQLiteAsyncConnection(Constants.DataPath);
            this.database.CreateTableAsync<Assessment>();
        }

        public async Task AddAsync(Assessment assessment)
        {
            await database.InsertAsync(assessment);
        }

        public async Task DeleteAsync(int assessmentId)
        {
            Assessment assessment = await GetByIdAsync(assessmentId);
            if (assessment != null && assessment.assessmentId == assessmentId)
            {
                await database.DeleteAsync(assessment);
            }
        }

        public async Task<List<Assessment>> GetAllAsync()
        {
            return await this.database.Table<Assessment>().ToListAsync();
        }

        public async Task<List<Assessment>> GetByCourseId(int courseId)
        {
            return await database.Table<Assessment>().Where(x => x.courseId == courseId).ToListAsync();
        }

        public async Task<Assessment> GetByIdAsync(int assessmentId)
        {
            return await database.Table<Assessment>().Where(x => x.assessmentId == assessmentId).FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(int assessmentId, Assessment assessment)
        {
            if (assessmentId == assessment.assessmentId)
            {
                await database.UpdateAsync(assessment);
            }
        }
    }
}
