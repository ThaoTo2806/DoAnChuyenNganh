using QuanLyMaWinApp.ViewModels;

namespace QuanLyMaWinApp.Views;

public partial class AddProductPage : ContentPage
{
	public AddProductPage()
	{
		InitializeComponent();
        BindingContext = new AddProductViewModel();
    }
}