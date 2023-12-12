using AcademicPlanner.CoreEntities;
using AcademicPlanner.Pages;
using AcademicPlanner.UseCases.Interface;
using AcademicPlanner.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using Plugin.LocalNotification;
using System.Text.RegularExpressions;

namespace AcademicPlanner.ViewModel
{
    public partial class CourseViewModel : ObservableObject
    {
        private readonly ICourseUseCases _courseUseCases;
        private readonly ITermUseCases _termUseCases;
        [ObservableProperty]
        public ObservableCollection<Course> courseCollection;
        [ObservableProperty]
        public string text;
        [ObservableProperty]
        public Term selectedTerm;
        [ObservableProperty]
        public Course selectedCourse;
        [ObservableProperty]
        public string pageTitle;

        public CourseViewModel(ICourseUseCases useCases, ITermUseCases termUseCase)
        {
            _courseUseCases = useCases;
            _termUseCases = termUseCase;
            courseCollection = new ObservableCollection<Course>();
        }

        public async Task SetSelectedTerm(int termId)
        {
            SelectedTerm = await _termUseCases.ExecuteGetTermByIdAsync(termId);
        }

        public async Task SetSelectedCourse(int courseId)
        {
            SelectedCourse = await _courseUseCases.ExecuteGetCourseByIdAsync(courseId);
        }

        [RelayCommand]
        async Task Delete(Course course)
        {
            await _courseUseCases.ExecuteRemoveAsync(course);
            await LoadCoursesAsync();
        }

        [RelayCommand]
        async Task Add()
        {
            if (Text == null || CourseCollection == null)
                return;

            if(CourseCollection.Count >= 6)
            {
                await App.Current.MainPage.DisplayAlert("Error", "Each term can contain a maximum of 6 courses", "OK");
                return;
            }

            Course newCourse = new Course();
            newCourse.TermId = SelectedTerm.TermId;
            newCourse.Name = Text;
            newCourse.StartDate = DateTime.Today;
            newCourse.EndDate = DateTime.Today;   
            newCourse.CourseStatus = "Not Started";
            newCourse.InstructorName = "Instructor Name";
            newCourse.InstuctorEmail = "instuctor@wgu.edu";
            newCourse.InstuctorPhone = "111-111-1111";
            await _courseUseCases.ExecuteAddAsync(newCourse);
            Text = "";
            await LoadCoursesAsync();
        }

        public async Task LoadCoursesAsync()
        {
            if (SelectedTerm == null) return;

            CourseCollection.Clear();
            var buffer = await _courseUseCases.ExecuteGetCoursesByTermIdAsync(SelectedTerm.TermId);

            if (buffer == null) return;
            foreach (Course course in buffer)
            {
                CourseCollection.Add(course);
            }
        }

        [RelayCommand]
        public async Task GoToAssessments(Course course)
        {
            string s = course.CourseId.ToString();
            await Shell.Current.GoToAsync($"{nameof(AssessmentsPage)}?courseId={s}");
        }

        [RelayCommand]
        public async Task GoToTerms()
        {
            await Shell.Current.GoToAsync("//" + nameof(TermsPage));
        }

        [RelayCommand]
        public async Task ToggleEditMode(Course course)
        {
            string s = course.CourseId.ToString();
            await Shell.Current.GoToAsync($"{nameof(EditCoursePage)}?courseId={s}");
        }

        [RelayCommand]
        public async Task UpdateCourseAsync()
        {
            if (!await CanUpdate())
                return;

            string s = SelectedCourse.TermId.ToString();
            await _courseUseCases.ExecuteUpdateAsync(SelectedCourse);
            SelectedCourse = null;
            await Shell.Current.GoToAsync($"{nameof(CoursesPage)}?termId={s}");
        }

        private async Task<bool> CanUpdate()
        {
            if (string.IsNullOrEmpty(SelectedCourse.Name))
            {
                await App.Current.MainPage.DisplayAlert("Error", "Course name cannot be blank.", "OK");
                return false;
            }
            
            if (SelectedCourse.StartDate.Date > SelectedCourse.EndDate.Date)
            {
                await App.Current.MainPage.DisplayAlert("Error", "The course cannot start after the due date.", "OK");
                return false;
            }
            if (SelectedCourse.EndDate.Date < SelectedCourse.StartDate.Date)
            {
                await App.Current.MainPage.DisplayAlert("Error", "The course cannot end before the start date.", "OK");
                return false;
            }

            if (string.IsNullOrEmpty(SelectedCourse.InstructorName))
            {
                await App.Current.MainPage.DisplayAlert("Error", "Instructor name cannot be blank.", "OK");
                return false;
            }

            if (string.IsNullOrEmpty(SelectedCourse.InstuctorEmail) || !IsValidEmail(SelectedCourse.InstuctorEmail))
            {
                await App.Current.MainPage.DisplayAlert("Error", "Instructor email must be in a valid format", "OK");
                return false;
            }

            if(string.IsNullOrEmpty(SelectedCourse.InstuctorPhone) || !IsValidPhoneNumber(SelectedCourse.InstuctorPhone))
            {
                await App.Current.MainPage.DisplayAlert("Error", "Instructor phone number must be in the format 555-555-5555", "OK");
                return false;
            }
            return true;
        }
        private bool IsValidEmail(string email)
        {
            string emailPattern = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";
            return Regex.IsMatch(email, emailPattern);
        }

        private bool IsValidPhoneNumber(string phoneNumber)
        {
            // This pattern matches the phone number format 800-555-5555
            string phonePattern = @"^\d{3}-\d{3}-\d{4}$";

            return Regex.IsMatch(phoneNumber, phonePattern);
        }

        public void SetPageTitle()
        {
            if(SelectedTerm == null) return;
            PageTitle = SelectedTerm.TermTitle + " Courses";
        }

        [RelayCommand]
        public async Task CancelEdit()
        {
            string s = SelectedCourse.TermId.ToString();
            SelectedCourse = null;
            await Shell.Current.GoToAsync($"{nameof(CoursesPage)}?termId={s}");
        }

        [RelayCommand]
        public async Task GoToCourseNotes(Course course)
        {
            string s = course.CourseId.ToString();
            await Shell.Current.GoToAsync($"{nameof(CourseNotesPage)}?courseId={s}");
        }

        public async Task DateNotify()
        {
            if (CourseCollection == null) return;

            foreach (Course course in CourseCollection) 
            {
                if (course.StartDate.Month == DateTime.Now.Month && course.StartDate.Day == DateTime.Now.Day)
                {
                    var request = new NotificationRequest
                    {
                        NotificationId = 1,
                        Title = course.Name + " Starts Today.",
                        Description = "Make sure to review the course contents and reach out to your instructor with any questions.",
                        Schedule = new NotificationRequestSchedule
                        {
                            NotifyTime = DateTime.Now,
                        }
                    };
                    await LocalNotificationCenter.Current.Show(request);
                }
                if (course.EndDate.Date >= DateTime.Now.Date && course.EndDate.Date <= DateTime.Now.AddDays(5).Date)
                {
                    int daysDifference = (course.EndDate.Date - DateTime.Now.Date).Days;
                    var request = new NotificationRequest
                    {
                        NotificationId = 2,
                        Title = course.Name + " ends in " + daysDifference + " days.", 
                        Description = "Make sure to submit all assessments before the course ends.",
                        Schedule = new NotificationRequestSchedule
                        {
                            NotifyTime = DateTime.Now,
                        }
                    };
                    await LocalNotificationCenter.Current.Show(request);
                }

            }
        }

        [RelayCommand]
        public async Task ShareText(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                await App.Current.MainPage.DisplayAlert("Error", "You cannot share a blank note.", "OK");
                return;
            }
            await Share.Default.RequestAsync(new ShareTextRequest
            {
                Text = text,
                Title = "Share Text"
            });
        }


    }
}
