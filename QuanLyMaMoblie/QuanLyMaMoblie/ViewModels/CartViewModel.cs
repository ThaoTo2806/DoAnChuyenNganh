using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Maui.Controls;
using QuanLyMaMoblie.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using QuanLyMaMoblie.Views;

namespace QuanLyMaMoblie.ViewModels
{
    public partial class CartViewModel : ObservableObject
    {
        private readonly Member _member;

        [ObservableProperty]
        private int _dem;

        [ObservableProperty]
        private ObservableCollection<CartItem> _productsInCart;

        [ObservableProperty]
        private double _totalPrice;

        public ICommand GoBackCommand { get; }
        public ICommand MoveToPayCommand { get; }
        public ICommand IncrementQuantityCommand { get; }
        public ICommand DecrementQuantityCommand { get; }
        public ICommand RemoveProductCommand { get; }

        public CartViewModel() : this(new Member())
        {
        }

        public CartViewModel(Member member)
        {
            _member = member;
            GoBackCommand = new Command(async () => await GoBackAsync());
            MoveToPayCommand = new Command(async () => await MoveToPayAsync());
            IncrementQuantityCommand = new Command<CartItem>(async (item) => await IncrementQuantityAsync(item));
            DecrementQuantityCommand = new Command<CartItem>(async (item) => await DecrementQuantityAsync(item));
            RemoveProductCommand = new Command<CartItem>(async (item) => await RemoveProductAsync(item));
            ProductsInCart = new ObservableCollection<CartItem>();

            InitializeCartAsync();
        }

        private async Task InitializeCartAsync()
        {
            Dem = await _member.GetTotalQuantity();
            if (Dem == 0)
            {
                await Application.Current.MainPage.DisplayAlert("Cart", "Your cart is empty.", "OK");
            }
            else
            {
                var cart = await _member.GetCartAsync();
                if (cart != null)
                {
                    ProductsInCart.Clear();
                    foreach (var item in cart.Items)
                    {
                        ProductsInCart.Add(item);
                    }
                    UpdateTotalPrice();
                }
            }
        }

        private void UpdateTotalPrice()
        {
            TotalPrice = ProductsInCart.Sum(item => item.TotalPrice);
            Dem = ProductsInCart.Sum(item => item.Quantity);
        }

        private async Task GoBackAsync()
        {
            try
            {
                await Application.Current.MainPage.Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private async Task MoveToPayAsync()
        {
            try
            {
                await Application.Current.MainPage.Navigation.PushAsync(new PayMentView());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                await Application.Current.MainPage.DisplayAlert("Error", "An error occurred while adding to cart.", "OK");
            }
        }

        public async Task<bool> IncrementQuantityAsync(CartItem item)
        {
            if (item != null)
            {
                item.Quantity++;
                UpdateTotalPrice();
                OnPropertyChanged(nameof(Dem));
                return await _member.IncrementQuantityAsync(item.ProductId);
            }
            return false;
        }

        public async Task<bool> DecrementQuantityAsync(CartItem item)
        {
            if (item != null && item.Quantity > 0)
            {
                item.Quantity--;
                UpdateTotalPrice();
                OnPropertyChanged(nameof(Dem));
                return await _member.DecrementQuantityAsync(item.ProductId);
            }
            return false;
        }
        public async Task<bool> RemoveProductAsync(CartItem item)
        {
            if (item != null)
            {
                bool success = await _member.RemoveFromCartAsync(item.ProductId);
                if (success)
                {
                    ProductsInCart.Remove(item);
                    Dem = await _member.GetTotalQuantity();
                    UpdateTotalPrice();
                }
                return success;
            }
            return false;
        }
    }
}
