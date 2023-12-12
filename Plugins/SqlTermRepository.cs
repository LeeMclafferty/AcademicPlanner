using AcademicPlanner.CoreEntities;
using AcademicPlanner.UseCases.Interface;
using SQLite;

namespace DataStore.SQLite
{
    public class SqlTermRepository : ISQLiteRepository
    {
        private SQLiteAsyncConnection database;

        public SqlTermRepository() 
        {
            // Create db if one does not exist.
            this.database = new SQLiteAsyncConnection(Constants.DataPath);
            this.database.CreateTableAsync<Term>();
            this.database.CreateTableAsync<Course>();
            this.database.CreateTableAsync<Assessment>();
            //_ = ClearDatabase();
        }

        public async Task AddTermAsync(Term term)
        {
            await database.InsertAsync(term);
        }

        public async Task DeleteTermAsync(int termId)
        {
            Term term = await GetTermByIdAsync(termId);
            if (term != null && term.TermId == termId)
            {
                // Fetch associated course IDs
                var courseIdList = await database.QueryAsync<int>("SELECT CourseId FROM Course WHERE TermId = ?", termId);

                if (courseIdList.Count > 0)
                {
                    // Fetch associated assessment IDs for these courses
                    var assessmentIdList = await database.QueryAsync<int>("SELECT assessmentId FROM Assessment WHERE courseId IN (?)", string.Join(",", courseIdList));

                    // Delete assessments
                    if (assessmentIdList.Count > 0)
                    {
                        await database.ExecuteAsync($"DELETE FROM Assessment WHERE assessmentId IN ({string.Join(",", assessmentIdList)})");
                    }

                    // Delete courses
                    await database.ExecuteAsync($"DELETE FROM Course WHERE CourseId IN ({string.Join(",", courseIdList)})");
                }

                // Delete the term
                await database.DeleteAsync(term);
            }
        }


        public async Task<List<Term>> GetTermsAsync()
        {
            return await this.database.Table<Term>().ToListAsync();
        }

        public async Task<Term> GetTermByIdAsync(int termId)
        {
            return await database.Table<Term>().Where(x => x.TermId == termId).FirstOrDefaultAsync();
        }

        public async Task UpdateTermAsync(int termId, Term term)
        {
            if(termId == term.TermId)
            {
                await database.UpdateAsync(term);
            }
        }

        public async Task InitializeDefaultTermsAsync()
        {
            await this.database.CreateTableAsync<Term>();
            await this.database.CreateTableAsync<Course>();
            await this.database.CreateTableAsync<Assessment>();
            // Check if there are any terms in the database
            var existingTerms = await database.Table<Term>().CountAsync();

            // If there are no terms in the database, add 3 default terms
            if (existingTerms == 0)
            {
                await database.ExecuteAsync("DELETE FROM Term");
                await database.ExecuteAsync("DELETE FROM Course");
                await database.ExecuteAsync("DELETE FROM Assessment");

                var defaultTerm = new Term { TermId = 1, TermTitle = "Term 1", StartDate = DateTime.Now, EndDate = DateTime.Now.AddMonths(4) };
                await database.ExecuteAsync("INSERT INTO Term (TermId, TermTitle, StartDate, EndDate) VALUES (?, ?, ?, ?)", defaultTerm.TermId, defaultTerm.TermTitle, defaultTerm.StartDate, defaultTerm.EndDate);

                var defaultCourse = new Course { CourseId = 1, TermId = 1, Name = "Intro To C++", StartDate = DateTime.Now, EndDate = DateTime.Now.AddMonths(4), InstructorName = "Anika Patel", InstuctorEmail = "anika.patel@strimeuniversity.edu", InstuctorPhone = "555-123-4567" };
                await database.ExecuteAsync("INSERT INTO Course (CourseId, TermId, Name, StartDate, EndDate, InstructorName, InstuctorEmail, InstuctorPhone) VALUES (?, ?, ?, ?, ?, ?, ?, ?)", defaultCourse.CourseId, defaultCourse.TermId, defaultCourse.Name, defaultCourse.StartDate, defaultCourse.EndDate, defaultCourse.InstructorName, defaultCourse.InstuctorEmail, defaultCourse.InstuctorPhone);

                var defaultAssessment1 = new Assessment { assessmentId = 1, courseId = 1, assessmentName = "C++ Syntax", assessmentType = "Objective Assessment", StartDate = DateTime.Now.AddMonths(1), EndDate = DateTime.Now.AddMonths(4) };
                await database.ExecuteAsync("INSERT INTO Assessment (assessmentId, courseId, assessmentName, assessmentType, StartDate, EndDate) VALUES (?, ?, ?, ?, ?, ?)", defaultAssessment1.assessmentId, defaultAssessment1.courseId, defaultAssessment1.assessmentName, defaultAssessment1.assessmentType, defaultAssessment1.StartDate, defaultAssessment1.EndDate);

                var defaultAssessment2 = new Assessment { assessmentId = 998, courseId = 1, assessmentName = "C++ File System", assessmentType = "Performance Assessment", StartDate = DateTime.Now, EndDate = DateTime.Now.AddMonths(4) };
                await database.ExecuteAsync("INSERT INTO Assessment (assessmentId, courseId, assessmentName, assessmentType, StartDate, EndDate) VALUES (?, ?, ?, ?, ?, ?)", defaultAssessment2.assessmentId, defaultAssessment2.courseId, defaultAssessment2.assessmentName, defaultAssessment2.assessmentType, defaultAssessment2.StartDate, defaultAssessment2.EndDate);
            }

        }

        public async Task ClearDatabase()
        {
            await database.ExecuteAsync(@"DELETE FROM Term");
            await database.ExecuteAsync(@"DELETE FROM Course");
            await database.ExecuteAsync(@"DELETE FROM Assessment");
        }

    }
}
