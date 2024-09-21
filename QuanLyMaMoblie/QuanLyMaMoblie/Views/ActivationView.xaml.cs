using Microsoft.Maui.Controls;
using QuanLyMaMoblie.Models;
using QuanLyMaMoblie.ViewModels;
using System.Collections.Generic;

namespace QuanLyMaMoblie.Views
{
    public partial class ActivationView : ContentPage
    {
        public ActivationView(List<ExpiredOrder> expiredOrders)
        {
            InitializeComponent();
            var viewModel = new ActivationViewModel(expiredOrders);
            BindingContext = viewModel;
        }
    }
}
