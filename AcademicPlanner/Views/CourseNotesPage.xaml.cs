using AcademicPlanner.ViewModel;

namespace AcademicPlanner.Views;

[QueryProperty(nameof(courseIdString), "courseId")]
public partial class CourseNotesPage : ContentPage
{
    private readonly CourseViewModel _courseViewModel;
	public CourseNotesPage(CourseViewModel vm)
	{
		InitializeComponent();
        _courseViewModel = vm;
        BindingContext = _courseViewModel;
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _courseViewModel.SetSelectedCourse(CourseId);
    }

    public int CourseId { get; set; }
    public string courseIdString
    {
        set
        {
            if (!string.IsNullOrWhiteSpace(value) && int.TryParse(value, out int courseId))
            {
                CourseId = courseId;
            }
        }
    }
}