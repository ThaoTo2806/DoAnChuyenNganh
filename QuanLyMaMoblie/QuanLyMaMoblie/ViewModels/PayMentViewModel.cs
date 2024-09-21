using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using QuanLyMaMoblie.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using QuanLyMaMoblie.Views;

namespace QuanLyMaMoblie.ViewModels
{
    public partial class PayMentViewModel : ObservableObject
    {
        private readonly Member _member;

        [ObservableProperty]
        private string name;

        [ObservableProperty]
        private string address;

        [ObservableProperty]
        private string phone;

        [ObservableProperty]
        private List<CartItem> cartItems;

        [ObservableProperty]
        private double totalPrice;

        [ObservableProperty]
        private string activationCodeInfo;

        [ObservableProperty]
        private bool isNapasSelected;

        [ObservableProperty]
        private bool isCashSelected;

        [ObservableProperty]
        private string paymentStatus;

        public ICommand ContinueCommand { get; }

        public ICommand GoBackCommand { get; }

        public PayMentViewModel() : this(new Member())
        {
        }

        public PayMentViewModel(Member member)
        {
            _member = member;
            GoBackCommand = new Command(async () => await GoBackAsync());
            ContinueCommand = new Command(async () => await ContinueAsync());

            // Đặt trạng thái mặc định cho phương thức thanh toán
            IsNapasSelected = true;  // Mặc định chọn Napas hoặc logic khác để chọn phương thức thanh toán mặc định
            PaymentStatus = "Napas";

            InitializeAsync();
        }

        private async Task InitializeAsync()
        {
            try
            {
                var orderInfo = await _member.GetOrderInfoAsync();

                if (orderInfo != null)
                {
                    Name = orderInfo.Name;
                    Address = orderInfo.Address;
                    Phone = orderInfo.Phone;
                    CartItems = orderInfo.CartItems;
                    TotalPrice = orderInfo.TotalPrice;
                    ActivationCodeInfo = orderInfo.ActivationCodeInfo;

                    // Đặt trạng thái chọn mặc định dựa trên PaymentStatus
                    IsNapasSelected = PaymentStatus == "Napas";
                    IsCashSelected = PaymentStatus == "Cash";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi lấy thông tin đơn hàng: {ex.Message}");
            }
        }

        private async Task GoBackAsync()
        {
            try
            {
                await Application.Current.MainPage.Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi trong quá trình điều hướng về trang trước: {ex.Message}");
            }
        }

        private async Task ContinueAsync()
        {
            try
            {
                PaymentStatus = IsNapasSelected ? "Napas" : "Credit";
                var deliveryAddress = Address;

                var success = await _member.CreateOrderAsync(deliveryAddress, PaymentStatus);
                if (success!=null)
                {
                    //await Application.Current.MainPage.DisplayAlert("Success", "Order placed successfully!", "OK");
                    await Application.Current.MainPage.Navigation.PushAsync(new IndexView());
                }
                else
                {
                   // await Application.Current.MainPage.DisplayAlert("Error", "Failed to place order. Please try again.", "OK");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi tạo đơn hàng: {ex.Message}");
            }
        }

    }
}
