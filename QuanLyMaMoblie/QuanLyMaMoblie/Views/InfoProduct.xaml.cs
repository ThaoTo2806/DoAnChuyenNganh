using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Maui.Controls;
using QuanLyMaMoblie.Models;
using QuanLyMaMoblie.ViewModels;
using System;

namespace QuanLyMaMoblie.Views
{
    public partial class InfoProduct : ContentPage
    {
        public InfoProduct(Product product)
        {
            InitializeComponent();
            BindingContext = new InfoProductViewModel(product);
        }
    }
}
