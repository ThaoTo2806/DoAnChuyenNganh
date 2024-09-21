namespace QuanLyMaWinApp.Views;
using QuanLyMaWinApp.ViewModels;

public partial class OrderView : ContentView
{
    public OrderView()
    {
        InitializeComponent();
        BindingContext = new OrderViewModel();
    }
}