using Microsoft.Maui.Controls;
using QuanLyMaMoblie.Models;
using QuanLyMaMoblie.ViewModels;

namespace QuanLyMaMoblie.Views
{
    public partial class CartView : ContentPage
    {
        public CartView()
        {
            InitializeComponent();
            BindingContext = new CartViewModel();
        }
    }
}