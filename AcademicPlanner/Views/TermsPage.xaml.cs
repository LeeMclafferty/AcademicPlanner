using AcademicPlanner.ViewModel;

namespace AcademicPlanner
{
    public partial class TermsPage : ContentPage
    {
        private readonly TermViewModel _termViewModel;
        public TermsPage(TermViewModel vm)
        {
            InitializeComponent();
            _termViewModel = vm;
            BindingContext = _termViewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _termViewModel.InitDatabase();
            await _termViewModel.LoadTermsAsync();
        }
    }
}