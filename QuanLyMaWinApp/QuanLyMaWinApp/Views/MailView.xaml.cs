namespace QuanLyMaWinApp.Views;
using QuanLyMaWinApp.ViewModels;
using QuanLyMaWinApp.Models;

public partial class MailView : ContentView
{
	public MailView()
	{
		InitializeComponent();
        BindingContext = new MailViewModel();
    }
    private async void OnSeeTapped(object sender, EventArgs e)
    {
        var image = sender as Image;
        var activationRequest = image?.BindingContext as ActivationRequest;
        if (activationRequest != null)
        {
            await (BindingContext as MailViewModel)?.OnSee(activationRequest);
        }
    }

}