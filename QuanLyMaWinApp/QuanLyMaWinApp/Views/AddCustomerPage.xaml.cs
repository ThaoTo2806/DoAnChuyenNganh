using QuanLyMaWinApp.ViewModels;

namespace QuanLyMaWinApp.Views;


public partial class AddCustomerPage : ContentPage
{
	public AddCustomerPage()
	{
		InitializeComponent();
        BindingContext = new AddCustomerViewModel();
    }
}