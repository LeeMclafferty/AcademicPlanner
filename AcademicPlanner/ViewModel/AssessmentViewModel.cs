using AcademicPlanner.CoreEntities;
using AcademicPlanner.Pages;
using AcademicPlanner.UseCases.Interface;
using AcademicPlanner.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Plugin.LocalNotification;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace AcademicPlanner.ViewModel
{
    public partial class AssessmentViewModel : ObservableObject
    {
        private readonly IAssessmentUseCases _assessmentUseCases;
        private readonly ICourseUseCases _courseUseCases;
        private readonly ITermUseCases _termUseCases;
        
        [ObservableProperty]
        public ObservableCollection<Assessment> assessmentCollection;
        [ObservableProperty]
        public Course selectedCourse;
        [ObservableProperty]
        public Assessment selectedAssessment;
        [ObservableProperty]
        public string text;

        public AssessmentViewModel(IAssessmentUseCases assessmentUseCases, ICourseUseCases courseUseCases, ITermUseCases termUseCase) 
        { 
            _assessmentUseCases = assessmentUseCases;
            _courseUseCases = courseUseCases;
            _termUseCases = termUseCase;
            assessmentCollection = new ObservableCollection<Assessment>();
        }

        public async Task SetSelectedCourse(int courseId)
        {
            SelectedCourse = await _courseUseCases.ExecuteGetCourseByIdAsync(courseId);
        }

        public async Task SetSelectedAssessment(int assessmentId)
        {
            SelectedAssessment = await _assessmentUseCases.ExecuteGetByIdAsync(assessmentId);
        }

        [RelayCommand]
        public async Task GoToCoursesPage()
        {
            string s = SelectedCourse.TermId.ToString();
            await Shell.Current.GoToAsync($"{nameof(CoursesPage)}?termId={s}");
        }

        public async Task LoadAssessments()
        {
            if (SelectedCourse == null) return;

            AssessmentCollection.Clear();
            var buffer = await _assessmentUseCases.ExecuteGetByCourseIdAsync(SelectedCourse.CourseId);

            if (buffer == null) return;
            foreach (Assessment assessment in buffer)
            {
                AssessmentCollection.Add(assessment);
            }
        }

        [RelayCommand]
        public async Task UpdateAsync()
        {
            if (!await CanUpdate())
                return;

            
            string s = SelectedCourse.CourseId.ToString();
            await _assessmentUseCases.ExecuteUpdateAsync(SelectedAssessment);
            SelectedAssessment = null;
            await Shell.Current.GoToAsync($"{nameof(AssessmentsPage)}?courseId={s}");
        }

        private async Task<bool> CanUpdate()
        {
            if(string.IsNullOrEmpty(SelectedAssessment.assessmentName))
            {
                await App.Current.MainPage.DisplayAlert("Error", "Assessment name cannot be blank.", "OK");
                return false;
            }
            
            if(AssessmentCollection ==  null) await LoadAssessments();

            foreach (Assessment assessment in AssessmentCollection)
            {
                if(SelectedAssessment.assessmentType == assessment.assessmentType && SelectedAssessment.assessmentId != assessment.assessmentId)
                {
                    await App.Current.MainPage.DisplayAlert("Error", "You can only have one " + assessment.assessmentType + " per course.", "OK");
                    return false;
                }
            }

            if(SelectedAssessment.StartDate.Date > SelectedAssessment.EndDate.Date)
            {
                await App.Current.MainPage.DisplayAlert("Error", "The assessment cannot start after the due date.", "OK");
                return false;
            }
            if (SelectedAssessment.EndDate.Date < SelectedAssessment.StartDate.Date)
            {
                await App.Current.MainPage.DisplayAlert("Error", "The assessment cannot end before the start date.", "OK");
                return false;
            }

            return true;
        }

        [RelayCommand]
        public async Task CancelEdit()
        {
            string s = SelectedCourse.CourseId.ToString();
            SelectedAssessment = null;
            await Shell.Current.GoToAsync($"{nameof(AssessmentsPage)}?courseId={s}");
        }

        [RelayCommand]
        async Task Add()
        {
            if (Text == null)
                return;
            if (AssessmentCollection.Count >= 2)
            {
                await App.Current.MainPage.DisplayAlert("Error", "Each course can only have two assessments, one OA and one PA", "OK");
                return;
            }

            Assessment newAssessment = new Assessment();
            if(AssessmentCollection.Count == 0) 
            {
                newAssessment.assessmentType = "Objective Assessment";
            }
            else
            {
                foreach(Assessment assessment in AssessmentCollection)
                { 
                    if(assessment.assessmentType == "Objective Assessment")
                    {
                        newAssessment.assessmentType = "Performance Assessment";
                    }
                    else
                    {
                        newAssessment.assessmentType = "Objective Assessment";
                    }
                }
            }
            newAssessment.StartDate = DateTime.Today;
            newAssessment.EndDate = DateTime.Today;
            newAssessment.assessmentName = Text;
            newAssessment.courseId = SelectedCourse.CourseId;
            await _assessmentUseCases.ExecuteAddAsync(newAssessment);
            Text = "";
            await LoadAssessments();
        }

        [RelayCommand]
        public async Task ToggleEditMode(Assessment assessment)
        {
            string s = assessment.assessmentId.ToString();
            await Shell.Current.GoToAsync($"{nameof(EditAssessmentPage)}?assessmentId={s}");
        }

        [RelayCommand]
        async Task Delete(Assessment assessment)
        {
            await _assessmentUseCases.ExecuteRemoveAsync(assessment);
            await LoadAssessments();
        }

        public async Task DateNotify()
        {
            if (AssessmentCollection == null) return;

            foreach (Assessment assessment in AssessmentCollection)
            {
                if (assessment.StartDate.Month == DateTime.Now.Month && assessment.StartDate.Day == DateTime.Now.Day)
                {
                    var request = new NotificationRequest
                    {
                        NotificationId = 1,
                        Title = assessment.assessmentName + " Starts Today.",
                        Description = "Make sure to review the Assessment contents and reach out to your instructor with any questions.",
                        Schedule = new NotificationRequestSchedule
                        {
                            NotifyTime = DateTime.Now,
                        }
                    };
                    await LocalNotificationCenter.Current.Show(request);
                }
                if (assessment.EndDate.Date >= DateTime.Now.Date && assessment.EndDate.Date <= DateTime.Now.AddDays(5).Date)
                {
                    int daysDifference = (assessment.EndDate.Date - DateTime.Now.Date).Days;
                    var request = new NotificationRequest
                    {
                        NotificationId = 2,
                        Title = assessment.assessmentName + " ends in " + daysDifference + " days.",
                        Description = "Make sure to submit all assessments before the Assessment ends.",
                        Schedule = new NotificationRequestSchedule
                        {
                            NotifyTime = DateTime.Now,
                        }
                    };
                    await LocalNotificationCenter.Current.Show(request);
                }


            }
        }
    }
}
