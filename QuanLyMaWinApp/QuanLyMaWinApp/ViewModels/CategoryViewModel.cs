using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using QuanLyMaWinApp.Views;
using QuanLyMaWinApp.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace QuanLyMaWinApp.ViewModels
{
    public class CategoryViewModel : ObservableObject
    {
        private const int PageSize = 5;
        private int _currentPage = 1;
        private bool _isBusy;
        private List<CategoryModel> _allCategories = new List<CategoryModel>();

        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        public ObservableCollection<CategoryModel> Categories { get; set; } = new ObservableCollection<CategoryModel>();

        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand AddCommand { get; }
        public IAsyncRelayCommand LoadCategoriesCommand { get; }
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

        public CategoryViewModel()
        {
            EditCommand = new Command<CategoryModel>(async (category) => await OnEdit(category));
            DeleteCommand = new Command<CategoryModel>(async (category) => await OnDelete(category));
            AddCommand = new Command(async () => await OnAddAsync());
            LoadCategoriesCommand = new AsyncRelayCommand(LoadCategoriesAsync);
            LoadMoreCommand = new Command(async () => await LoadMoreCategoriesAsync());

            NextPageCommand = new Command(async () => await GoToNextPageAsync());
            PreviousPageCommand = new Command(async () => await GoToPreviousPageAsync());

            LoadCategoriesAsync();
        }

        private async Task LoadCategoriesAsync()
        {
            try
            {
                IsBusy = true;
                _currentPage = 1;
                _allCategories = await CategoryModel.GetCategoriesAsync();
                Categories.Clear();
                foreach (var category in _allCategories.Take(PageSize))
                {
                    Categories.Add(category);
                }

                CanGoToNextPage = _allCategories.Count > PageSize;
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

        private async Task LoadMoreCategoriesAsync()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;
                var nextCategories = _allCategories.Skip(_currentPage * PageSize).Take(PageSize);

                foreach (var category in nextCategories)
                {
                    Categories.Add(category);
                }

                CanGoToNextPage = nextCategories.Any();
                _currentPage++;
                CanGoToPreviousPage = _currentPage > 1;
                OnPropertyChanged(nameof(CurrentPageLabel));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while loading more categories: {ex.Message}");
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
                var nextCategories = _allCategories.Skip((_currentPage - 1) * PageSize).Take(PageSize).ToList();

                Categories.Clear();
                foreach (var category in nextCategories)
                {
                    Categories.Add(category);
                }

                CanGoToNextPage = _currentPage * PageSize < _allCategories.Count;
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
                var previousCategories = _allCategories.Skip((_currentPage - 1) * PageSize).Take(PageSize);

                Categories.Clear();
                foreach (var category in previousCategories)
                {
                    Categories.Add(category);
                }

                CanGoToNextPage = _currentPage * PageSize < _allCategories.Count;
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

        private async Task OnDelete(CategoryModel category)
        {
            if (category != null)
            {
                // Hiển thị hộp thoại xác nhận xóa
                var result = await Application.Current.MainPage.DisplayAlert(
                    "Confirm Delete",
                    $"Are you sure you want to delete {category.CategoryName}?",
                    "Yes",
                    "No"
                );

                if (result)
                {
                    try
                    {
                        // Gọi phương thức SoftDeleteCategoryAsync để đánh dấu danh mục là đã xóa
                        bool success = await CategoryModel.SoftDeleteCategoryAsync(category.ID);
                        if (success)
                        {
                            // Cập nhật danh sách danh mục
                            _allCategories.Remove(category);
                            Categories.Remove(category);
                            await GoToPreviousPageAsync();
                        }
                        else
                        {
                            await Application.Current.MainPage.DisplayAlert("Error", "Could not delete the category.", "OK");
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
                var addCategoryPage = new AddCategoryPage();
                addCategoryPage.Disappearing += async (s, e) =>
                {
                    await LoadCategoriesAsync();
                };
                await Application.Current.MainPage.Navigation.PushAsync(addCategoryPage);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error when redirecting to AddCategory page: {ex.Message}");
                await Application.Current.MainPage.DisplayAlert("Error", "Unable to move to add Category page.", "OK");
            }
        }

        private async Task OnEdit(CategoryModel category)
        {
            try
            {
                if (category != null)
                {
                    var editCategoryPage = new EditCategoryPage(category);
                    editCategoryPage.Disappearing += async (s, e) =>
                    {
                        await LoadCategoriesAsync();
                    };
                    await Application.Current.MainPage.Navigation.PushAsync(editCategoryPage);
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
