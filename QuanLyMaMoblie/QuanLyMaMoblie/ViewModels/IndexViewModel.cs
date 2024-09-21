using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using QuanLyMaMoblie.Models;
using QuanLyMaMoblie.Views;

namespace QuanLyMaMoblie.ViewModels
{
    public partial class IndexViewModel : ObservableObject
    {
        private Member _member;

        private ObservableCollection<Product> _products;
        public ObservableCollection<Product> Products
        {
            get => _products;
            set => SetProperty(ref _products, value);
        }
        [ObservableProperty]
        private int _dem;

        public ICommand FrameTappedCommand { get; }
        public ICommand HomeCommand { get; }
        public ICommand NoteCommand { get; }
        public ICommand MailCommand { get; }
        public ICommand UserCommand { get; }
        public ICommand MoveToCartCommand { get; }

        public IndexViewModel()
        {
            _member = new Member();

            FrameTappedCommand = new Command<Product>(OnFrameTapped);
            HomeCommand = new Command(async () => await NavigateToHome());
            NoteCommand = new Command(async () => await NavigateToNote());
            MailCommand = new Command(async () => await NavigateToMail());
            UserCommand = new Command(async () => await NavigateToUser());
            MoveToCartCommand = new Command(async () => await MoveToCart());

            Products = new ObservableCollection<Product>();

            // Load data asynchronously
            Task.Run(async () => await LoadDataAsync());
        }

        private async void OnFrameTapped(Product product)
        {
            try
            {
                product.FrameColor = Colors.Gray;
                await Application.Current.MainPage.Navigation.PushAsync(new InfoProduct(product));
                product.FrameColor = Colors.White;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error handling frame tapped event: {ex.Message}");
            }
        }

        private async Task NavigateToHome()
        {
            try
            {
                await Application.Current.MainPage.Navigation.PushAsync(new IndexView());
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error navigating to Home: {ex.Message}");
            }
        }

        private async Task NavigateToNote()
        {
            try
            {
                await Application.Current.MainPage.Navigation.PushAsync(new AllOrdersView());
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error navigating to Note: {ex.Message}");
            }
        }

        private async Task NavigateToMail()
        {
            try
            {
                await Application.Current.MainPage.Navigation.PushAsync(new MailView());
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error navigating to Mail: {ex.Message}");
            }
        }

        private async Task NavigateToUser()
        {
            try
            {
                await Application.Current.MainPage.Navigation.PushAsync(new UserView());
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error navigating to User: {ex.Message}");
            }
        }

        public async Task LoadDataAsync()
        {
            try
            {
                // Gọi GetTotalQuantity và gán giá trị cho Dem
                Dem = await _member.GetTotalQuantity();
                // Lấy danh sách sản phẩm
                var products = await Product.GetProductsAsync();
                Products.Clear();
                foreach (var product in products)
                {
                    Products.Add(product);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading data: {ex.Message}");
            }
        }
        private async Task MoveToCart()
        {
            try
            {
                await Application.Current.MainPage.Navigation.PushAsync(new CartView());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                await Application.Current.MainPage.DisplayAlert("Error", "An error occurred while adding to cart.", "OK");
            }
        }
    }
}
