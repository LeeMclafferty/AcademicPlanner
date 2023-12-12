using AcademicPlanner.ViewModel;
using Plugin.LocalNotification;

namespace AcademicPlanner.Pages;

[QueryProperty(nameof(courseIdString), "courseId")]
public partial class AssessmentsPage : ContentPage
{
    private readonly AssessmentViewModel _assessmentViewModel;
	public AssessmentsPage(AssessmentViewModel vm)
	{
		InitializeComponent();
        _assessmentViewModel = vm;
        BindingContext = _assessmentViewModel;
	}

    protected override async void OnAppearing()
    {
        // Need to pass multiple query params and set the selected term as well so I can pass it to the course page when going back. 
        base.OnAppearing();
        await _assessmentViewModel.SetSelectedCourse(CourseId);
        await _assessmentViewModel.LoadAssessments();

        if (!await LocalNotificationCenter.Current.AreNotificationsEnabled())
            await LocalNotificationCenter.Current.RequestNotificationPermission();

        await _assessmentViewModel.DateNotify();
    }

    public int CourseId { get; set; }
    public string courseIdString
    {
        set
        {
            if (!string.IsNullOrWhiteSpace(value) && int.TryParse(value, out int intId))
            {
                CourseId = intId;
            }
        }
    }
}