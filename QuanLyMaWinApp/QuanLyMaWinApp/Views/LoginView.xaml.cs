namespace QuanLyMaWinApp.Views;
using QuanLyMaWinApp.ViewModels;

public partial class LoginView : ContentPage
{
	public LoginView()
	{
		InitializeComponent();
        BindingContext = new LoginViewModel();
    }

    private void OnSubmitClicked(object sender, EventArgs e)
    {
        // Điều này sẽ gọi phương thức OnSubmit trong LoginViewModel
        (BindingContext as LoginViewModel)?.OnSubmitCommand.Execute(null);

    }
}