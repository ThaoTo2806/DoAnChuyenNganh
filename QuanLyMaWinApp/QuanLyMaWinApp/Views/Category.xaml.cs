namespace QuanLyMaWinApp.Views;
using QuanLyMaWinApp.ViewModels;

public partial class Category : ContentView
{
	public Category()
	{
		InitializeComponent();
        BindingContext = new CategoryViewModel();
    }
}