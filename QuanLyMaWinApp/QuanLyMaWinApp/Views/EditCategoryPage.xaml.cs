namespace QuanLyMaWinApp.Views;

using QuanLyMaWinApp.Models;
using QuanLyMaWinApp.ViewModels;

public partial class EditCategoryPage : ContentPage
{
	public EditCategoryPage(CategoryModel categoryModel)
	{
		InitializeComponent();
		BindingContext = new EditCategoryViewModel(categoryModel);
	}
}