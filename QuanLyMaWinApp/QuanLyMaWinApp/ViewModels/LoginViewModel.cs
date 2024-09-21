using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using QuanLyMaWinApp.Models;
using QuanLyMaWinApp.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using CommunityToolkit.Mvvm.Input;

namespace QuanLyMaWinApp.ViewModels
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

        public LoginViewModel()
        {
            OnSubmitCommand = new RelayCommand(async () => await OnSubmit());
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
                    await DieuHuongDenIndexView();
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Invalid credentials or not authorized", "OK");
                }
            }
            catch (Exception ex)
            {
                // Log the exception if necessary
                Console.WriteLine($"An error occurred: {ex.Message}");
                await Application.Current.MainPage.DisplayAlert("Error", "An error occurred while attempting to login. Please try again later.", "OK");
            }
        }

        private async Task DieuHuongDenIndexView()
        {
            var indexView = new MainPageView();
            await ((NavigationPage)App.Current.MainPage).PushAsync(indexView);
        }
    }
}
