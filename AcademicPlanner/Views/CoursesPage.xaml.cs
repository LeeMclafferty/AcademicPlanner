using AcademicPlanner.ViewModel;
using Plugin.LocalNotification;
using System.Threading.Tasks;

namespace AcademicPlanner.Pages;

[QueryProperty(nameof(termIdString), "termId")]
public partial class CoursesPage : ContentPage
{
    private readonly CourseViewModel _courseViewModel;

    public CoursesPage(CourseViewModel vm)
	{
		InitializeComponent();
        _courseViewModel = vm;
        BindingContext = _courseViewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _courseViewModel.SetSelectedTerm(TermId);
        _courseViewModel.SetPageTitle();
        await _courseViewModel.LoadCoursesAsync();

        if(!await LocalNotificationCenter.Current.AreNotificationsEnabled())
            await LocalNotificationCenter.Current.RequestNotificationPermission();
        
        await _courseViewModel.DateNotify();
    }

    public int TermId { get; set; }
    public string termIdString
    {
        set
        {
            if (!string.IsNullOrWhiteSpace(value) && int.TryParse(value, out int termID))
            {
                TermId = termID;
            }
        }
    }
}