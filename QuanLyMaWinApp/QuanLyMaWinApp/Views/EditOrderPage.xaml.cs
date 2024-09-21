using Microsoft.Maui.Controls;
using QuanLyMaWinApp.ViewModels;
using System.Threading.Tasks;

namespace QuanLyMaWinApp.Views
{
    public partial class EditOrderPage : ContentPage
    {
        public EditOrderPage(int orderId)
        {
            InitializeComponent();

            // Khởi tạo ViewModel với orderId
            var viewModel = new OrderDetailViewModel(orderId);
            BindingContext = viewModel;

            // Tải chi tiết đơn hàng
            viewModel.LoadOrderDetailsAsync();
        }
    }

}
