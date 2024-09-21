using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using QuanLyMaMoblie.Models;
using QuanLyMaMoblie.Views;
using System.Windows.Input;

namespace QuanLyMaMoblie.ViewModels
{
    public partial class ForgotPassViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _account;

        [ObservableProperty]
        private string _email;

        public ICommand OnSubmitCommand { get; }
        public ICommand GoBackCommand { get; }

        public ForgotPassViewModel()
        {
            OnSubmitCommand = new RelayCommand(async () => await OnSubmit());
            GoBackCommand = new Command(async () => await GoBack());
        }

        private async Task GoBack()
        {
            try
            {
                await Application.Current.MainPage.Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", $"No return: {ex.Message}", "OK");
            }
        }

        private async Task OnSubmit()
        {
            if (string.IsNullOrWhiteSpace(Account) || string.IsNullOrWhiteSpace(Email))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Please enter both account and email.", "OK");
                return;
            }

            try
            {
                // Create an instance of UpdatePasswordRequest
                var updatePasswordRequest = new UpdatePasswordRequest
                {
                    Account = Account,
                    Email = Email
                };

                var member = new Member();
                // Pass the UpdatePasswordRequest object to ForgotPassAsync
                bool isSuccess = await member.ForgotPassAsync(updatePasswordRequest);
                if (isSuccess)
                {
                    await Application.Current.MainPage.DisplayAlert("Notification", "New password updated! Please check your email", "OK");
                    await Application.Current.MainPage.Navigation.PushAsync(new LoginView());
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Password or account is incorrect!", "OK");
                }
            }
            catch (Exception ex)
            {
                // Log error if needed
                Console.WriteLine($"An error occurred: {ex.Message}");
                await Application.Current.MainPage.DisplayAlert("Error", "An error occurred while attempting to login. Please try again later.", "OK");
            }
        }

    }
}
