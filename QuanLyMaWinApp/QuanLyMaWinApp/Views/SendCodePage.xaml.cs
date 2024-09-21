namespace QuanLyMaWinApp.Views;

using QuanLyMaWinApp.Models;
using QuanLyMaWinApp.ViewModels;

public partial class SendCodePage : ContentPage
{
	public SendCodePage(ActivationRequest mailModel)
	{
		InitializeComponent();
        BindingContext = new SendCodeViewModel(mailModel);
    }
}