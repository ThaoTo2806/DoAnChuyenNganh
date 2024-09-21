using QuanLyMaWinApp.ViewModels;

namespace QuanLyMaWinApp.Views;


public partial class AddCategoryPage : ContentPage
{
    public AddCategoryPage()
    {
        InitializeComponent();
        BindingContext = new AddCategoryViewModel();
    }
}