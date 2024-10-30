using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using QuanLyMaMoblie.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Runtime.InteropServices;
using QuanLyMaMoblie.Views;

namespace QuanLyMaMoblie.ViewModels
{
    public partial class ActivationViewModel : ObservableObject
    {
        private List<ExpiredOrder> _expiredOrdersList;

        public List<ExpiredOrder> ExpiredOrdersList
        {
            get => _expiredOrdersList;
            set => SetProperty(ref _expiredOrdersList, value);
        }

        public Member _member;
        private Member _profile;
        public Member Profile
        {
            get => _profile;
            set => SetProperty(ref _profile, value);
        }

        [ObservableProperty]
        private bool isNapasSelected;

        [ObservableProperty]
        private bool isCashSelected;

        [ObservableProperty]
        private bool is1Selected;

        [ObservableProperty]
        private bool is3Selected;

        [ObservableProperty]
        private bool is8Selected;

        [ObservableProperty]
        private double price;

        public ICommand GoBackCommand { get; }
        public ICommand SendCommand { get; }

        public ActivationViewModel()
        {
            GoBackCommand = new Command(async () => await GoBackAsync());
            SendCommand = new AsyncRelayCommand(SendRequest);
            // Đăng ký lắng nghe thay đổi thuộc tính
            PropertyChanged += OnPropertyChanged;
        }

        public ActivationViewModel(List<ExpiredOrder> expiredOrders) : this()
        {
            ExpiredOrdersList = expiredOrders;
            _ = LoadOrdersAsync();
        }

        private async Task SendRequest()
        {
            // Tạo đối tượng ActivationRequest
            var activationRequest = new ActivationRequest
            {
                OrderID = ExpiredOrdersList[0].OrderId,
                Title = $"Require activation code for OrderId: {ExpiredOrdersList[0].OrderId}",
                Username = Profile.Name,
                Email = Profile.Email,
                PayMent = IsNapasSelected ? "Napas" : "Credit",
                RequestDay = DateTime.Now,
                Periodic = Is1Selected ? 1 :
                           Is3Selected ? 3 :
                           Is8Selected ? 8 : 0,
                Total = Price
            };
            try
            {
                _member = new Member();
                bool result = await _member.RequestActivationAsync(activationRequest);

                // Hiển thị thông báo sau khi gửi yêu cầu
                if (result)
                {
                    await Application.Current.MainPage.DisplayAlert("Request Sent", "Your request has been sent", "OK");
                    await Application.Current.MainPage.Navigation.PushAsync(new MailView());
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Request Failed", "There was a problem sending your request", "OK");
                }
            }
            catch (HttpRequestException httpEx)
            {
                await Application.Current.MainPage.DisplayAlert("Request Error", $"HTTP Request Error: {httpEx.Message}", "OK");
            }
            catch (TaskCanceledException taskEx)
            {
                await Application.Current.MainPage.DisplayAlert("Request Error", $"Request Timed Out: {taskEx.Message}", "OK");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Unexpected Error", $"An unexpected error occurred: {ex.Message}", "OK");
            }
        }


        private async Task LoadOrdersAsync()
        {
            try
            {
                _member = new Member();
                await Task.Delay(2000);
                ExpiredOrdersList = await _member.GetExpritedCodeOrdersAsync();
                Profile = await _member.GetProfileAsync();
                IsNapasSelected = true;
                Is1Selected = true;
                Price = 89.99;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading data: {ex.Message}");
            }
        }

        private void OnPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Is1Selected))
            {
                if (Is1Selected)
                {
                    Price = 89.99;
                    Is3Selected = false;
                    Is8Selected = false;
                }
            }
            else if (e.PropertyName == nameof(Is3Selected))
            {
                if (Is3Selected)
                {
                    Price = 267.90;
                    Is1Selected = false;
                    Is8Selected = false;
                }
            }
            else if (e.PropertyName == nameof(Is8Selected))
            {
                if (Is8Selected)
                {
                    Price = 719.00;
                    Is1Selected = false;
                    Is3Selected = false;
                }
            }
        }
        private async Task GoBackAsync()
        {
            // Navigate to IndexView
            await Application.Current.MainPage.Navigation.PushAsync(new MailView());
        }
    }
}
