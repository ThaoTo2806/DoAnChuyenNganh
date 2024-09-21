namespace QuanLyMaMoblie.Views;
using QuanLyMaMoblie.ViewModels;
using QuanLyMaMoblie.Models;
public partial class InfoUser : ContentPage
{
	public InfoUser(Member member)
	{
		InitializeComponent();
		BindingContext = new InfoUserViewModel(member);
    }
}