using AcademicPlanner.Pages;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using AcademicPlanner.UseCases.Interface;
using AcademicPlanner.CoreEntities;
using AcademicPlanner.Views;

namespace AcademicPlanner.ViewModel
{
    public partial class TermViewModel : ObservableObject
    {
        private readonly ITermUseCases _termUseCases;
        [ObservableProperty]
        public ObservableCollection<Term> termsCollection;
        [ObservableProperty]
        string text;
        [ObservableProperty]
        public Term selectedTerm;

        public TermViewModel(ITermUseCases termUseCases) 
        {
            _termUseCases = termUseCases;
            TermsCollection = new ObservableCollection<Term>();
        }

        [RelayCommand]
        async Task Delete(Term term)
        {
            await _termUseCases.ExecuteRemoveAsync(term);
            await LoadTermsAsync();
        }

        [RelayCommand]
        async Task Tap(string s)
        {
            await Shell.Current.GoToAsync($"{nameof(CoursesPage)}?Text={s}");
        }

        [RelayCommand]
        async Task Add()
        {
            if (Text == null)
                return;

            Term newTerm = new Term();
            newTerm.TermTitle = Text;
            newTerm.StartDate = DateTime.Now;
            newTerm.EndDate = DateTime.Now;
            await _termUseCases.ExecuteAddAsync(newTerm);
            Text = "";
            await LoadTermsAsync();
        }

        public async Task LoadTermsAsync()
        {
            TermsCollection.Clear();
            var buffer = await _termUseCases.ExecuteGetTermsAsync();

            foreach (Term term in buffer)
            {
                if(!TermsCollection.Any(t => t.TermId == term.TermId))
                    TermsCollection.Add(term);
            }
        }

        [RelayCommand]
        public async Task UpdateTermAsync()
        {
            if (!await CanUpdate())
                return;

            await _termUseCases.ExecuteUpdateAsync(SelectedTerm);
            await Shell.Current.GoToAsync("..");
        }

        private async Task<bool> CanUpdate()
        {
            if (string.IsNullOrEmpty(SelectedTerm.TermTitle))
            {
                await App.Current.MainPage.DisplayAlert("Error", "Term title cannot be blank.", "OK");
                return false;
            }

            if (SelectedTerm.StartDate.Date > SelectedTerm.EndDate.Date)
            {
                await App.Current.MainPage.DisplayAlert("Error", "The term cannot start after the due date.", "OK");
                return false;
            }
            if (SelectedTerm.EndDate.Date < SelectedTerm.StartDate.Date)
            {
                await App.Current.MainPage.DisplayAlert("Error", "The term cannot end before the start date.", "OK");
                return false;
            }

            return true;
        }

        [RelayCommand]
        public async Task ToggleEditMode(Term term)
        {
            string s = term.TermId.ToString();
            await Shell.Current.GoToAsync($"{nameof(EditTermPage)}?termId={s}");
        }

        public async Task<Term> GetTermByIdAsync(int termId)
        {
            return await _termUseCases.ExecuteGetTermByIdAsync(termId);
        }

        [RelayCommand]
        public async Task GotoCourse(Term term)
        {
            string s = term.TermId.ToString();
            await Shell.Current.GoToAsync($"{nameof(CoursesPage)}?termId={s}");
        }

        public async Task SetSelectedTerm(int id)
        {
            Term buffer = await GetTermByIdAsync(id);
            SelectedTerm = buffer;
        }

        public async Task InitDatabase()
        {
            await _termUseCases.ExecuteInitDatabase();
        }
    }
}
