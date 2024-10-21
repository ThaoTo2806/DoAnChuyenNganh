using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using QuanLyMaWinApp.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace QuanLyMaWinApp.ViewModels
{
    public class EditProductViewModel : ObservableObject
    {
        private ProductModel _proModel;
        private Dictionary<string, string> _categoryNameToIdMap = new Dictionary<string, string>();

        public string ID
        {
            get => _proModel?.ID;
            set
            {
                if (_proModel != null && _proModel.ID != value)
                {
                    _proModel.ID = value;
                }
            }
        }

        public string Name
        {
            get => _proModel?.Name;
            set
            {
                if (_proModel != null && _proModel.Name != value)
                {
                    _proModel.Name = value;
                    OnPropertyChanged();
                }
            }
        }

        public int Sl
        {
            get => _proModel?.SL ?? 0;
            set
            {
                if (_proModel != null && _proModel.SL != value)
                {
                    _proModel.SL = value;
                    OnPropertyChanged();
                }
            }
        }

        public int Version
        {
            get => _proModel?.IdVersion?? 0;
            set
            {
                if (_proModel != null && _proModel.IdVersion != value)
                {
                    _proModel.IdVersion = value;
                    OnPropertyChanged();
                }
            }
        }

        public double price
        {
            get => _proModel?.Price ?? 0;
            set
            {
                if (_proModel != null && _proModel.Price != value)
                {
                    _proModel.Price = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Detail
        {
            get => _proModel?.Detail;
            set
            {
                if (_proModel != null && _proModel.Detail != value)
                {
                    _proModel.Detail = value;
                    OnPropertyChanged();
                }
            }
        }

        public string ft
        {
            get => _proModel?.Feature;
            set
            {
                if (_proModel != null && _proModel.Feature != value)
                {
                    _proModel.Feature = value;
                    OnPropertyChanged();
                }
            }
        }

        public string sp
        {
            get => _proModel?.Specifications;
            set
            {
                if (_proModel != null && _proModel.Specifications != value)
                {
                    _proModel.Specifications = value;
                    OnPropertyChanged();
                }
            }
        }

        public string hp
        {
            get => _proModel?.Helps;
            set
            {
                if (_proModel != null && _proModel.Helps != value)
                {
                    _proModel.Helps = value;
                    OnPropertyChanged();
                }
            }
        }

        private ImageSource _selectedImage;
        public ICommand ChooseImageCommand => new AsyncRelayCommand(OnChooseImageClicked);

        public ImageSource SelectedImage
        {
            get => _selectedImage;
            set => SetProperty(ref _selectedImage, value);
        }

        public ObservableCollection<string> CategoryIDs { get; } = new ObservableCollection<string>();

        public ICommand SaveCommand { get; }

        public EditProductViewModel()
        {
            SaveCommand = new AsyncRelayCommand(SaveExecute);
        }

        public EditProductViewModel(ProductModel proModel) : this()
        {
            _proModel = proModel;
            _ = LoadCategoriesAsync(); // Call the asynchronous method
        }

        private async Task OnChooseImageClicked()
        {
            try
            {
                var result = await MediaPicker.PickPhotoAsync();

                if (result != null)
                {
                    var stream = await result.OpenReadAsync();
                    SelectedImage = ImageSource.FromStream(() => stream);
                    string imagePath = result.FullPath;

                    _proModel.Image = imagePath;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error selecting image: {ex.Message}");
            }
        }

        private async Task SaveExecute()
        {
            try
            {

                bool success = await ProductModel.UpdateProductDetailsAsync(
                    ID,
                    Name,
                    _proModel.IdCate,
                    _proModel.Image,
                    Sl,
                    price,
                    Detail,
                    ft,
                    sp,
                    hp,
                    Version
                );

                if (success)
                {
                    await Application.Current.MainPage.DisplayAlert("Success", "Product updated successfully", "OK");

                    if (Application.Current.MainPage.Navigation.NavigationStack.Count > 1)
                    {
                        await Application.Current.MainPage.Navigation.PopAsync();
                    }
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Could not update the product", "OK");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An error occurred while updating the product: {ex.Message}");
                await Application.Current.MainPage.DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
        }

        private async Task LoadCategoriesAsync()
        {
            try
            {
                var categories = await CategoryModel.GetCategoriesAsync();
                CategoryIDs.Clear(); // Clear the collection first
                _categoryNameToIdMap.Clear(); // Clear the map
                foreach (var category in categories)
                {
                    CategoryIDs.Add(category.CategoryName);
                    _categoryNameToIdMap[category.CategoryName] = category.ID; // Populate the map
                }

                // Set the selected category
                if (_proModel != null && !string.IsNullOrEmpty(_proModel.IdCate))
                {
                    SelectedCategory = _proModel.IdCate;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An error occurred while loading categories: {ex.Message}");
            }
        }

        private string _selectedCategory;
        public string SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                if (_selectedCategory != value)
                {
                    _selectedCategory = value;
                    OnPropertyChanged();
                    // Update the category ID in the product model
                    if (_proModel != null && _categoryNameToIdMap.TryGetValue(value, out var categoryId))
                    {
                        _proModel.IdCate = categoryId;
                    }
                }
            }
        }
    }
}
