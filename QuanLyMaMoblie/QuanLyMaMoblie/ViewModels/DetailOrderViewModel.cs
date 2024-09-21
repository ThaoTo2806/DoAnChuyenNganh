using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using QuanLyMaMoblie.Models;
using QuanLyMaMoblie.Views;

namespace QuanLyMaMoblie.ViewModels
{
    public partial class DetailOrderViewModel : ObservableObject
    {
        private OrderDetails _order;
        public OrderDetails Order
        {
            get => _order;
            set => SetProperty(ref _order, value);
        }

        public ObservableCollection<ProductDetails> Products { get; } = new ObservableCollection<ProductDetails>();

        private readonly Member _member;
        public ICommand NoteCommand { get; }

        private int _orderId;
        public int OrderId
        {
            get => _orderId;
            set => SetProperty(ref _orderId, value);
        }

        private Color _frameColor1;
        private Color _frameColor2;
        private Color _frameColor3;
        private Color _frameColor4;

        public Color FrameColor1
        {
            get => _frameColor1;
            set => SetProperty(ref _frameColor1, value);
        }

        public Color FrameColor2
        {
            get => _frameColor2;
            set => SetProperty(ref _frameColor2, value);
        }

        public Color FrameColor3
        {
            get => _frameColor3;
            set => SetProperty(ref _frameColor3, value);
        }

        public Color FrameColor4
        {
            get => _frameColor4;
            set => SetProperty(ref _frameColor4, value);
        }

        public DetailOrderViewModel()
        {
            NoteCommand = new AsyncRelayCommand(NavigateToNote);
        }

        public DetailOrderViewModel(int orderId)
        {
            _member = new Member();
            OrderId = orderId;
            LoadOrderDetailsAsync(orderId).ConfigureAwait(false);
            NoteCommand = new AsyncRelayCommand(NavigateToNote);
        }

        private async Task LoadOrderDetailsAsync(int orderId)
        {
            try
            {
                var userOrderDetails = await _member.GetUserOrderDetailsAsync(orderId);

                if (userOrderDetails != null)
                {
                    // Dữ liệu thông tin đơn hàng chung
                    Order = userOrderDetails.Order;

                    UpdateFrameColors1();
                    UpdateFrameColors2();

                    // Danh sách sản phẩm
                    Products.Clear();
                    foreach (var product in userOrderDetails.Products)
                    {
                        Products.Add(product);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading order details: {ex.Message}");
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

        private void UpdateFrameColors1()
        {
            if (Order == null) return;

            switch (Order.OrderStatus)
            {
                case "Processing":
                    FrameColor3 = Colors.Blue;
                    FrameColor4 = Colors.White;
                    break;
                case "Confirmed":
                    FrameColor3 = Colors.Orange;
                    FrameColor4 = Colors.Black;
                    break;
                case "Cancelled":
                    FrameColor3 = Colors.Red;
                    FrameColor4 = Colors.White;
                    break;
                case "Delivered":
                    FrameColor3 = Colors.Green;
                    FrameColor4 = Colors.White;
                    break;
                default:
                    FrameColor3 = Colors.Transparent;
                    FrameColor4 = Colors.Black;
                    break;
            }
        }

        private void UpdateFrameColors2()
        {
            if (Order == null) return;

            switch (Order.PaymentStatus)
            {
                case "Credit":
                    FrameColor1 = Colors.Pink;
                    FrameColor2 = Colors.Black;
                    break;
                case "Napas":
                    FrameColor1 = Colors.Yellow;
                    FrameColor2 = Colors.Black;
                    break;
                default:
                    FrameColor1 = Colors.Transparent;
                    FrameColor2 = Colors.Black;
                    break;
            }
        }
    }
}
