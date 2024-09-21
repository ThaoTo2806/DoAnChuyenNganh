using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using QuanLyMaMoblie.Models;
using QuanLyMaMoblie.Views;
using System.Windows.Input;

namespace QuanLyMaMoblie.ViewModels
{
    public partial class RegisterViewModel : ObservableObject
    {
        [ObservableProperty]
        private string fullNameEntry;

        [ObservableProperty]
        private string emailEntry;

        [ObservableProperty]
        private string phoneEntry;

        [ObservableProperty]
        private string accountEntry;

        [ObservableProperty]
        private string passwordEntry;

        [ObservableProperty]
        private string confirmPasswordEntry;

        public Member MemberInfo { get; }
        public ICommand OnSubmitCommand { get; }
        public ICommand OnLoginCommand { get; }

        public RegisterViewModel()
        {
            MemberInfo = new Member();
            OnSubmitCommand = new RelayCommand(async () => await OnSubmit());
            OnLoginCommand = new Command(async () => await OnLogin());
        }

        private async Task OnSubmit()
        {
            try
            {
                // Kiểm tra các trường nhập liệu có null hoặc trắng không
                if (string.IsNullOrWhiteSpace(FullNameEntry) || string.IsNullOrWhiteSpace(EmailEntry) || string.IsNullOrWhiteSpace(PhoneEntry) ||
                    string.IsNullOrWhiteSpace(AccountEntry) || string.IsNullOrWhiteSpace(PasswordEntry) ||
                    string.IsNullOrWhiteSpace(ConfirmPasswordEntry))
                {
                    await App.Current.MainPage.DisplayAlert("Error", "Please enter complete information", "OK");
                    return;
                }

                // Kiểm tra mật khẩu và xác nhận mật khẩu có trùng khớp không
                if (PasswordEntry != ConfirmPasswordEntry)
                {
                    await App.Current.MainPage.DisplayAlert("Error", "Password and confirmation password do not match", "OK");
                    return;
                }

                var member = new Member();
                bool isSuccess = await member.RegisterAsync(FullNameEntry, EmailEntry, AccountEntry, PasswordEntry, PhoneEntry);

                if (isSuccess)
                {
                    await App.Current.MainPage.DisplayAlert("Notification", "Sign Up Successfully", "OK");
                    await Application.Current.MainPage.Navigation.PushAsync(new LoginView());
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Error", "Account already exists or registration failed!", "OK");
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
        }

        private async Task OnLogin()
        {
            try
            {
                // Điều hướng sang trang đăng nhập
                await Application.Current.MainPage.Navigation.PushAsync(new LoginView());
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
        }
    }
}
