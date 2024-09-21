using Microsoft.Maui.Controls;
using QuanLyMaMoblie.Models;
using QuanLyMaMoblie.ViewModels;

namespace QuanLyMaMoblie.Views
{
    public partial class PayMentView : ContentPage
    {
        public PayMentView()
        {
            InitializeComponent();
            BindingContext = new PayMentViewModel();
        }
    }
} 