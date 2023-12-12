using AcademicPlanner.CoreEntities;
using AcademicPlanner.ViewModel;

namespace AcademicPlanner.Views;

[QueryProperty(nameof(IdString), "termId")]
public partial class EditTermPage : ContentPage
{
    private readonly TermViewModel _termViewModel;
    public EditTermPage(TermViewModel vm)
    {
        InitializeComponent();
        _termViewModel = vm;
        BindingContext = _termViewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _termViewModel.SetSelectedTerm(TermId);
    }

    public int TermId { get; set; }

    public string IdString
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