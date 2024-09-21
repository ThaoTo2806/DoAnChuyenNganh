using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using QuanLyMaMoblie.Models;
using QuanLyMaMoblie.Views;

namespace QuanLyMaMoblie.ViewModels
{
    public partial class UserViewModel : ObservableObject
    {
        private Member _member;
        [ObservableProperty]
        private Member _profile;
        public ICommand HomeCommand { get; }
        public ICommand NoteCommand { get; }
        public ICommand MailCommand { get; }
        public ICommand UserCommand { get; }
        public ICommand LogoutCommand { get; }

        public ICommand NavigateToInfoCommand { get; }

        public UserViewModel()
        {
            _member = new Member();
            LoadProfileAsync().ConfigureAwait(false); // Thực hiện gọi dữ liệu

            HomeCommand = new Command(async () => await NavigateToHome());
            NoteCommand = new Command(async () => await NavigateToNote());
            MailCommand = new Command(async () => await NavigateToMail());
            UserCommand = new Command(async () => await NavigateToUser());
            LogoutCommand = new Command(async () => await LogoutAsync());
            NavigateToInfoCommand = new Command(async () => await NavigateToInFoUser(Profile));
        }

        private async Task LoadProfileAsync()
        {
            try
            {
                Profile = await _member.GetProfileAsync();
            }
            catch (HttpRequestException ex)
            {
                Debug.WriteLine($"Request error while fetching profile: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Debug.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
            }
            catch (TaskCanceledException ex)
            {
                Debug.WriteLine($"Request timeout while fetching profile: {ex.Message}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unexpected error while fetching profile: {ex.Message}");
            }
        }

        private async Task NavigateToHome()
        {
            try
            {
                await Application.Current.MainPage.Navigation.PushAsync(new IndexView());
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error navigating to Home: {ex.Message}");
            }
        }

        private async Task NavigateToNote()
        {
            try
            {
                await Application.Current.MainPage.Navigation.PushAsync(new AllOrdersView());
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error navigating to Note: {ex.Message}");
            }
        }

        private async Task NavigateToMail()
        {
            try
            {
                await Application.Current.MainPage.Navigation.PushAsync(new MailView());
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error navigating to Mail: {ex.Message}");
            }
        }

        private async Task NavigateToUser()
        {
            try
            {
                await Application.Current.MainPage.Navigation.PushAsync(new UserView());
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error navigating to User: {ex.Message}");
            }
        }

        private async Task NavigateToInFoUser(Member member)
        {
            try
            {
                if(member != null)
                {
                    var editInfoUser = new InfoUser(member);
                    editInfoUser.Disappearing += async (s, e) =>
                    {
                        await LoadProfileAsync();
                    };
                    await Application.Current.MainPage.Navigation.PushAsync(editInfoUser);
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Error", "Cannot proceed to the next page", "OK");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error navigating to Info User: {ex.Message}");
            }
        }

        private async Task LogoutAsync()
        {
            try
            {
                await _member.LogoutAsync();

                await App.Current.MainPage.DisplayAlert("Success", "You have been logged out successfully.", "OK");

                await Application.Current.MainPage.Navigation.PushAsync(new FirstPageView());
            }
            catch (HttpRequestException ex)
            {
                Debug.WriteLine($"Request error while logging out: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Debug.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
                await App.Current.MainPage.DisplayAlert("Error", "An error occurred while logging out. Please try again.", "OK");
            }
            catch (TaskCanceledException ex)
            {
                Debug.WriteLine($"Request timeout while logging out: {ex.Message}");
                await App.Current.MainPage.DisplayAlert("Error", "The request timed out. Please try again.", "OK");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unexpected error while logging out: {ex.Message}");
                await App.Current.MainPage.DisplayAlert("Error", "An unexpected error occurred. Please try again.", "OK");
            }
        }
    }
}
