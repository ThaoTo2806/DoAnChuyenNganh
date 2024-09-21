using CommunityToolkit.Mvvm.ComponentModel;
using QuanLyMaWinApp.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace QuanLyMaWinApp.ViewModels
{
    public class OrderDetailViewModel : ObservableObject
    {
        private bool _isBusy;
        private List<OrderDetailModel> _orderDetails;
        private int _orderId;
        private string _productIds;
        private string _orderStatus;
        private bool _isProcess;
        private bool _isConfirm;

        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        public List<OrderDetailModel> OrderDetails
        {
            get => _orderDetails;
            set
            {
                SetProperty(ref _orderDetails, value);
                ProductIds = string.Join(", ", _orderDetails.Select(d => d.ProductID));
                UpdateStatusFlags(); // Cập nhật trạng thái của RadioButton
            }
        }

        public string ProductIds
        {
            get => _productIds;
            set => SetProperty(ref _productIds, value);
        }

        public string OrderStatus
        {
            get => _orderStatus;
            set
            {
                if (SetProperty(ref _orderStatus, value))
                {
                    UpdateStatusFlags(); // Cập nhật trạng thái của RadioButton khi OrderStatus thay đổi
                }
            }
        }

        public bool IsProcess
        {
            get => _isProcess;
            set => SetProperty(ref _isProcess, value);
        }

        public bool IsConfirm
        {
            get => _isConfirm;
            set => SetProperty(ref _isConfirm, value);
        }
        public ICommand SaveCommand { get; }

        public OrderDetailViewModel(int orderId)
        {
            _orderId = orderId;
            SaveCommand = new Command(async () => await SaveExecute());
        }

        public async Task LoadOrderDetailsAsync()
        {
            IsBusy = true;
            try
            {
                OrderDetails = await OrderModel.GetOrderDetailsByIdAsync(_orderId);
                // Giả sử OrderStatus được lấy từ OrderDetails, có thể cần phải điều chỉnh theo dữ liệu thực tế
                if (OrderDetails.Any())
                {
                    OrderStatus = OrderDetails.First().OrderStatus; // Ví dụ lấy trạng thái của chi tiết đơn hàng đầu tiên
                }
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ hoặc ghi lại
                Console.WriteLine($"An error occurred while fetching order details: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void UpdateStatusFlags()
        {
            IsProcess = OrderStatus == "Processing";
            IsConfirm = OrderStatus == "Confirmed";
        }

        private async Task SaveExecute()
        {
            if (IsConfirm)
            {
                try
                {
                    // Call UpdateOrderAsync to update the order
                    bool success = await OrderModel.UpdateOrderAsync(_orderId);

                    if (success)
                    {
                        await Application.Current.MainPage.DisplayAlert("Success", "Order updated successfully", "OK");

                        // Navigate back to the previous page
                        if (Application.Current.MainPage.Navigation.NavigationStack.Count > 1)
                        {
                            await Application.Current.MainPage.Navigation.PopAsync();
                        }
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Error", "Could not update the Order", "OK");
                    }
                }
                catch (Exception ex)
                {
                    // Log the error (if logging is set up)
                    Console.WriteLine($"An error occurred while updating the Order: {ex.Message}");

                    // Show an alert to the user
                    await Application.Current.MainPage.DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
                }
            }
        }
    }
}
