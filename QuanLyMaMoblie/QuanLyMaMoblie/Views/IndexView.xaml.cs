using Microsoft.Maui.Controls;
using QuanLyMaMoblie.ViewModels;

namespace QuanLyMaMoblie.Views
{
    public partial class IndexView : ContentPage
    {
        public IndexView()
        {
            InitializeComponent();

            BindingContext = new IndexViewModel();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            var viewModel = (IndexViewModel)BindingContext;
            if (viewModel != null)
            {
                await viewModel.LoadDataAsync();
            }
        }
    }
}
