using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using QuanLyMaMoblie.Models;
using QuanLyMaMoblie.Views;
using System;
using System.Linq; // Thêm namespace này để sử dụng Linq methods như FirstOrDefault
using System.Threading.Tasks;
using System.Windows.Input;

namespace QuanLyMaMoblie.ViewModels
{
    public partial class InfoProductViewModel : ObservableObject
    {
        private Product _product;

        public Product Product
        {
            get => _product;
            set => SetProperty(ref _product, value);
        }

        private int _dem;
        public int Dem
        {
            get => _dem;
            set => SetProperty(ref _dem, value);
        }

        private bool _isHighlightsVisible;
        public bool IsHighlightsVisible
        {
            get => _isHighlightsVisible;
            set => SetProperty(ref _isHighlightsVisible, value);
        }

        private bool _isFeatureVisible;
        public bool IsFeatureVisible
        {
            get => _isFeatureVisible;
            set => SetProperty(ref _isFeatureVisible, value);
        }

        private bool _isSpecificationsVisible;
        public bool IsSpecificationsVisible
        {
            get => _isSpecificationsVisible;
            set => SetProperty(ref _isSpecificationsVisible, value);
        }

        public ICommand GoBackCommand { get; }
        public ICommand ShowContentCommand { get; }
        public ICommand IncrementQuantityCommand { get; }
        public ICommand MoveToCartCommand { get; }
        public ICommand MoveToCart1Command { get; }

        public InfoProductViewModel()
        {
            GoBackCommand = new Command(async () => await GoBackAsync());
            ShowContentCommand = new Command<string>(async (param) => await ShowContent(param));
            IncrementQuantityCommand = new Command(async () => await IncrementQuantityAsync(Product));
            MoveToCartCommand = new Command(async () => await AddToCartAsync(Product));
            MoveToCart1Command = new Command(async () => await MoveToCart());

            // Khởi tạo dữ liệu ngay trong constructor
            InitializeAsync();
        }

        public InfoProductViewModel(Product product) : this()
        {
            Product = product;
        }

        private async Task InitializeAsync()
        {
            try
            {
                // Tìm kiếm `IndexViewModel` trong `Application.Current.MainPage` để lấy giá trị `Dem`
                var indexViewModel = Application.Current.MainPage.Navigation.NavigationStack
                    .OfType<IndexView>()
                    .Select(v => v.BindingContext as IndexViewModel)
                    .FirstOrDefault();

                if (indexViewModel != null)
                {
                    Dem = indexViewModel.Dem;
                }
                else
                {
                    Console.WriteLine("IndexViewModel not found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing view model: {ex.Message}");
            }
        }

        private async Task GoBackAsync()
        {
            await Application.Current.MainPage.Navigation.PopAsync();
        }

        private async Task ShowContent(string section)
        {
            switch (section)
            {
                case "Highlights":
                    IsHighlightsVisible = true;
                    IsFeatureVisible = false;
                    IsSpecificationsVisible = false;
                    break;
                case "Feature":
                    IsHighlightsVisible = false;
                    IsFeatureVisible = true;
                    IsSpecificationsVisible = false;
                    break;
                case "Specifications":
                    IsHighlightsVisible = false;
                    IsFeatureVisible = false;
                    IsSpecificationsVisible = true;
                    break;
                default:
                    IsHighlightsVisible = false;
                    IsFeatureVisible = false;
                    IsSpecificationsVisible = false;
                    break;
            }
        }

        private async Task IncrementQuantityAsync(Product product)
        {
            try
            {
                if (product != null)
                {
                    var member = new Member();
                    bool success = await member.AddToCartAsync(product.ID,1);
                    if (success)
                    {
                        Dem = await member.GetTotalQuantity(); // Cập nhật số lượng sau khi tăng
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Error", "Failed to add product to the cart.", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                await Application.Current.MainPage.DisplayAlert("Error", "An error occurred while incrementing quantity.", "OK");
            }
        }

        private async Task AddToCartAsync(Product product)
        {
            try
            {
                if (product != null)
                {
                    var member = new Member(); // Khởi tạo Member
                    bool success = await member.AddToCartAsync(product.ID, 1);
                    if (success)
                    {
                        Dem = await member.GetTotalQuantity(); // Cập nhật số lượng sau khi thêm vào giỏ hàng
                        await Application.Current.MainPage.Navigation.PushAsync(new CartView());
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Error", "Failed to add product to the cart.", "OK");
                    }
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Invalid product selected.", "OK");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                await Application.Current.MainPage.DisplayAlert("Error", "An error occurred while adding to cart.", "OK");
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
