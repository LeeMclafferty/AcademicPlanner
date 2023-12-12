using AcademicPlanner.ViewModel;
using Microsoft.Extensions.Logging;

using AcademicPlanner.Pages;
using AcademicPlanner.UseCases.Interface;
using DataStore.SQLite;
using AcademicPlanner.Views;
using AcademicPlanner.UseCases;
using Plugin.LocalNotification;
using CommunityToolkit.Maui;

namespace AcademicPlanner
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder.UseMauiApp<App>().ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            })
                .UseLocalNotification();

            builder.UseMauiCommunityToolkit();

            /*
             * These register classes and interface mappings with .NET Maui, so that it can automatically create
             * instances for dependency injection at runtime.
             */
            //Views
            builder.Services.AddSingleton<TermsPage>();
            builder.Services.AddTransient<CoursesPage>();
            builder.Services.AddTransient<AssessmentsPage>();
            builder.Services.AddTransient<EditTermPage>();
            builder.Services.AddTransient<EditCoursePage>();
            builder.Services.AddTransient<EditAssessmentPage>();
            builder.Services.AddTransient<CourseNotesPage>();
            builder.Services.AddTransient<Test>();

            // View Models
            builder.Services.AddSingleton<TermViewModel>();
            builder.Services.AddSingleton<AssessmentViewModel>();
            builder.Services.AddTransient<CourseViewModel>();

            //Interface mappings
            builder.Services.AddSingleton<ITermUseCases, TermUseCases>();
            builder.Services.AddSingleton<ISQLiteRepository, SqlTermRepository>();
            builder.Services.AddSingleton<ICourseUseCases, CourseUseCases>();
            builder.Services.AddSingleton<ISQLiteCourses, SqlCourseRepository>();
            builder.Services.AddSingleton<ISQLiteAssessments, SqlAssessmentRepository>();
            builder.Services.AddSingleton<IAssessmentUseCases, AssessmentUseCases>();
#if DEBUG
            builder.Logging.AddDebug();
#endif
            return builder.Build();
        }
    }
}