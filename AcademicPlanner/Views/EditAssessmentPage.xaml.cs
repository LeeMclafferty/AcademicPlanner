using AcademicPlanner.ViewModel;

namespace AcademicPlanner.Views;

[QueryProperty(nameof(assessmentIdString), "assessmentId")]
public partial class EditAssessmentPage : ContentPage
{
    private readonly AssessmentViewModel _assessmentViewModel;
    public EditAssessmentPage(AssessmentViewModel vm)
    {
        InitializeComponent();
        _assessmentViewModel = vm;
        BindingContext = _assessmentViewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _assessmentViewModel.SetSelectedAssessment(AssessmentId);
    }

    public int AssessmentId { get; set; }
    public string assessmentIdString
    {
        set
        {
            if (!string.IsNullOrWhiteSpace(value) && int.TryParse(value, out int Id))
            {
                AssessmentId = Id;
            }
        }
    }
}