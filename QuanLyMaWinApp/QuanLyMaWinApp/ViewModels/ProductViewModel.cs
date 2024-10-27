using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using QuanLyMaWinApp.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using QuanLyMaWinApp.Views;

namespace QuanLyMaWinApp.ViewModels
{
    public class ProductViewModel : ObservableObject
    {
        private const int PageSize = 5;
        private int _currentPage = 1;
        private bool _isBusy;
        private List<ProductDetail> _allProducts = new List<ProductDetail>();
        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        public ObservableCollection<ProductDetail> Products { get; set; } = new ObservableCollection<ProductDetail>();

        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand AddCommand { get; }
        public IAsyncRelayCommand LoadProductsCommand { get; }
        public ICommand LoadMoreCommand { get; }
        public ICommand NextPageCommand { get; }
        public ICommand PreviousPageCommand { get; }

        private bool _canGoToPreviousPage;
        public bool CanGoToPreviousPage
        {
            get => _canGoToPreviousPage;
            private set => SetProperty(ref _canGoToPreviousPage, value);
        }

        private bool _canGoToNextPage;
        public bool CanGoToNextPage
        {
            get => _canGoToNextPage;
            private set => SetProperty(ref _canGoToNextPage, value);
        }

        public string CurrentPageLabel => $"Page {_currentPage}";

        public ProductViewModel()
        {
            EditCommand = new Command<ProductDetail>(async (product) => await OnEdit(product));
            DeleteCommand = new Command<ProductDetail>(async (product) => await OnDelete(product));
            AddCommand = new Command(async () => await OnAddAsync());

            LoadProductsCommand = new AsyncRelayCommand(LoadProductsAsync);
            LoadMoreCommand = new Command(async () => await LoadMoreProductsAsync());
            NextPageCommand = new Command(async () => await GoToNextPageAsync());
            PreviousPageCommand = new Command(async () => await GoToPreviousPageAsync());

            LoadProductsAsync();
        }

        private async Task LoadProductsAsync()
        {
            try
            {
                IsBusy = true;
                _currentPage = 1;
                _allProducts = await ProductModel.GetProductsAsync();
                Products.Clear();
                foreach (var Product in _allProducts.Take(PageSize))
                {
                    Products.Add(Product);
                }

                CanGoToNextPage = _allProducts.Count > PageSize;
                CanGoToPreviousPage = false; // Reset to false since we are on the first page
                OnPropertyChanged(nameof(CurrentPageLabel));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while loading categories: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task LoadMoreProductsAsync()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;
                var nextProducts = _allProducts.Skip(_currentPage * PageSize).Take(PageSize);

                foreach (var Product in nextProducts)
                {
                    Products.Add(Product);
                }

                CanGoToNextPage = nextProducts.Any();
                _currentPage++;
                CanGoToPreviousPage = _currentPage > 1;
                OnPropertyChanged(nameof(CurrentPageLabel));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while loading more Products: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task GoToNextPageAsync()
        {
            if (!CanGoToNextPage || IsBusy)
                return;

            try
            {
                IsBusy = true;
                _currentPage++;
                var nextProducts = _allProducts.Skip((_currentPage - 1) * PageSize).Take(PageSize).ToList();

                Products.Clear();
                foreach (var Product in nextProducts)
                {
                    Products.Add(Product);
                }

                CanGoToNextPage = _currentPage * PageSize < _allProducts.Count;
                CanGoToPreviousPage = _currentPage > 1;
                OnPropertyChanged(nameof(CurrentPageLabel));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while loading the next page: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task GoToPreviousPageAsync()
        {
            if (!CanGoToPreviousPage || IsBusy)
                return;

            try
            {
                IsBusy = true;
                _currentPage--;
                var previousProducts = _allProducts.Skip((_currentPage - 1) * PageSize).Take(PageSize);

                Products.Clear();
                foreach (var category in previousProducts)
                {
                    Products.Add(category);
                }

                CanGoToNextPage = _currentPage * PageSize < _allProducts.Count;
                CanGoToPreviousPage = _currentPage > 1;
                OnPropertyChanged(nameof(CurrentPageLabel));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while loading the previous page: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }
        private async Task OnDelete(ProductDetail product)
        {
            if (product != null)
            {
                // Hiển thị hộp thoại xác nhận xóa
                var result = await Application.Current.MainPage.DisplayAlert(
                    "Confirm Delete",
                    $"Are you sure you want to delete {product.Name}?",
                    "Yes",
                    "No"
                );

                if (result)
                {
                    try
                    {
                        bool success = await ProductModel.SoftDeleteProductAsync(product.ID);
                        if (success)
                        {
                            // Cập nhật danh sách danh mục
                            _allProducts.Remove(product);
                            Products.Remove(product);
                            await GoToPreviousPageAsync();
                        }
                        else
                        {
                            await Application.Current.MainPage.DisplayAlert("Error", "Could not delete the product.", "OK");
                        }
                    }
                    catch (Exception ex)
                    {
                        await Application.Current.MainPage.DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
                    }
                }
            }
        }

        private async Task OnAddAsync()
        {
            try
            {
                var addProductPage = new AddProductPage();
                addProductPage.Disappearing += async (s, e) =>
                {
                    await LoadProductsAsync();
                };
                await Application.Current.MainPage.Navigation.PushAsync(addProductPage);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error when redirecting to AddProduct page: {ex.Message}");
                await Application.Current.MainPage.DisplayAlert("Error", "Unable to move to add Product page.", "OK");
            }
        }

        private async Task OnEdit(ProductDetail product)
        {
            try
            {
                if (product != null)
                {
                    var editProductPage = new EditProductPage(product);
                    editProductPage.Disappearing += async (s, e) =>
                    {
                        await LoadProductsAsync();
                    };
                    await Application.Current.MainPage.Navigation.PushAsync(editProductPage);
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Cannot proceed to the next page", "OK");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error! An error occurred. Please try again later: {ex.Message}");
            }
        }
    }
}
