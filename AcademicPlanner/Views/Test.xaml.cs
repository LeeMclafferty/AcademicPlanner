using AcademicPlanner.ViewModel;

namespace AcademicPlanner.Views;

public partial class Test : ContentPage
{
	private readonly CourseViewModel ViewModel;
	public Test(CourseViewModel vm)
	{
		InitializeComponent();
		ViewModel = vm;
	}
}