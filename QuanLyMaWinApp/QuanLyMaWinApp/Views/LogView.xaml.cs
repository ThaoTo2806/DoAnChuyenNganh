namespace QuanLyMaWinApp.Views;
using QuanLyMaWinApp.ViewModels;

public partial class LogView : ContentView
{
	public LogView()
	{
		InitializeComponent();

        BindingContext = new LogViewModel();
    }
}