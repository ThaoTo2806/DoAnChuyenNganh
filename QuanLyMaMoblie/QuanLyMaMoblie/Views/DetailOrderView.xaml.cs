namespace QuanLyMaMoblie.Views;
using Microsoft.Maui.Controls;
using QuanLyMaMoblie.Models;
using QuanLyMaMoblie.ViewModels;

public partial class DetailOrderView : ContentPage
{
    public DetailOrderView(int orderId)
    {
        InitializeComponent();
        var viewModel = new DetailOrderViewModel(orderId);
        BindingContext = viewModel;
    }
}