using AcademicPlanner.Pages;
using AcademicPlanner.Views;

namespace AcademicPlanner
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(TermsPage), typeof(TermsPage));
            Routing.RegisterRoute(nameof(EditTermPage), typeof(EditTermPage));

            Routing.RegisterRoute(nameof(CoursesPage), typeof(CoursesPage));
            Routing.RegisterRoute(nameof(EditCoursePage), typeof(EditCoursePage));
            Routing.RegisterRoute(nameof(CourseNotesPage), typeof(CourseNotesPage));

            Routing.RegisterRoute(nameof(AssessmentsPage), typeof(AssessmentsPage));
            Routing.RegisterRoute(nameof(EditAssessmentPage), typeof(EditAssessmentPage));

            Routing.RegisterRoute(nameof(Test), typeof(Test));
        }
    }
}