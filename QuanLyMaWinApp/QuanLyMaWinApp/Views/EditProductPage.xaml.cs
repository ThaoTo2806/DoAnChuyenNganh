namespace QuanLyMaWinApp.Views;

using QuanLyMaWinApp.Models;
using QuanLyMaWinApp.ViewModels;

public partial class EditProductPage : ContentPage
{
	public EditProductPage(ProductDetail productModel)
	{
		InitializeComponent();
        BindingContext = new EditProductViewModel(productModel);
    }
}