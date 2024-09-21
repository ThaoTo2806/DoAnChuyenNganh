using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using QuanLyMaMoblie.Models;
using QuanLyMaMoblie.Views;
using System.Windows.Input;

namespace QuanLyMaMoblie.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _taiKhoan;

        [ObservableProperty]
        private string _matKhau;

        [ObservableProperty]
        private bool _ghiNhoToi;

        public ICommand OnSubmitCommand { get; }
        public ICommand GoBackCommand { get; }
        public ICommand OnForgotPassCommand { get; }

        public LoginViewModel()
        {
            OnSubmitCommand = new RelayCommand(async () => await OnSubmit());
            GoBackCommand = new Command(async () => await GoBack());
            OnForgotPassCommand = new Command(async () => await OnForgot());
        }

        private async Task OnForgot()
        {
            try
            {
                await Application.Current.MainPage.Navigation.PushAsync(new ForgotPassView());
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
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
            if (string.IsNullOrWhiteSpace(TaiKhoan) || string.IsNullOrWhiteSpace(MatKhau))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Please enter both username and password.", "OK");
                return;
            }

            try
            {
                var member = new Member();
                bool isSuccess = await member.DangNhapAsync(TaiKhoan, MatKhau);
                if (isSuccess)
                {
                    await ShowLoadingPage();
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Password or account is incorrect!", "OK");
                }
            }
            catch (Exception ex)
            {
                // Ghi lại lỗi nếu cần
                Console.WriteLine($"An error occurred: {ex.Message}");
                await Application.Current.MainPage.DisplayAlert("Error", "An error occurred while attempting to login. Please try again later.", "OK");
            }
        }

        private async Task ShowLoadingPage()
        {
            try
            {
                var loadingPage = new LoadingPage();
                var loadingViewModel = new LoadingPageViewModel();
                loadingPage.BindingContext = loadingViewModel;

                await ((NavigationPage)App.Current.MainPage).PushAsync(loadingPage);
                await loadingViewModel.LoadDataAndNavigate();
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", $"Unable to navigate: {ex.Message}", "OK");
            }
        }
    }
}
