using CommunityToolkit.Mvvm.ComponentModel;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using QuanLyMaMoblie.Views;

namespace QuanLyMaMoblie.ViewModels
{
    public partial class LoadingPageViewModel : ObservableObject
    {
        public async Task LoadDataAndNavigate()
        {
            var indexViewModel = new IndexViewModel();
            await indexViewModel.LoadDataAsync();

            var indexView = new IndexView
            {
                BindingContext = indexViewModel
            };

            await Application.Current.MainPage.Navigation.PushAsync(indexView);
            Application.Current.MainPage.Navigation.RemovePage(Application.Current.MainPage.Navigation.NavigationStack[Application.Current.MainPage.Navigation.NavigationStack.Count - 2]);
        }
    }
}
