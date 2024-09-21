using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using QuanLyMaMoblie.Views;
using static System.Net.Mime.MediaTypeNames;

namespace QuanLyMaMoblie.ViewModels
{
    public partial class FirstPageViewModel : ObservableObject
    {
        public ICommand OnSubmitCommand { get; }
        public ICommand OnSubmitCommand1 { get; }

        public FirstPageViewModel()
        {
            OnSubmitCommand = new Command(async () => await OnSubmit());
            OnSubmitCommand1 = new Command(async () => await OnSubmit1());
        }

        private async Task OnSubmit()
        {
            try
            {
                // Điều hướng sang trang đăng nhập
                await Microsoft.Maui.Controls.Application.Current.MainPage.Navigation.PushAsync(new LoginView());
            }
            catch (Exception ex)
            {
                await Microsoft.Maui.Controls.Application.Current.MainPage.DisplayAlert("Lỗi", $"Có lỗi xảy ra: {ex.Message}", "OK");
            }
        }

        private async Task OnSubmit1()
        {
            try
            {
                // Điều hướng sang trang đăng ký
                await Microsoft.Maui.Controls.Application.Current.MainPage.Navigation.PushAsync(new RegisterView());
            }
            catch (Exception ex)
            {
                await Microsoft.Maui.Controls.Application.Current.MainPage.DisplayAlert("Lỗi", $"Có lỗi xảy ra: {ex.Message}", "OK");
            }
        }

    }
}
