﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using QuanLyMaWinApp.Models;

namespace QuanLyMaWinApp.ViewModels
{
    public class AddProductViewModel : ObservableObject
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
                    OnPropertyChanged();
                }
            }
        }

        public string Name
        {
            get => _proModel?.ProductName;
            set
            {
                if (_proModel != null && _proModel.ProductName != value)
                {
                    _proModel.ProductName = value;
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

        public AddProductViewModel()
        {
            SaveCommand = new AsyncRelayCommand(SaveExecute);
            _proModel = new ProductModel();
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
                bool success = await ProductModel.InsertProductAsync(
                    ID,
                    Name,
                    _proModel.ldCate,
                    _proModel.Image,
                    Sl,
                    price,
                    Detail,
                    ft,
                    sp,
                    hp
                );

                if (success)
                {
                    await Application.Current.MainPage.DisplayAlert("Success", "Product created successfully", "OK");

                    if (Application.Current.MainPage.Navigation.NavigationStack.Count > 1)
                    {
                        await Application.Current.MainPage.Navigation.PopAsync();
                    }
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Could not create the product", "OK");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An error occurred while updating the product: {ex.Message}");
                await Application.Current.MainPage.DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
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
                    if (_proModel != null && _categoryNameToIdMap.TryGetValue(value, out var categoryId))
                    {
                        _proModel.ldCate = categoryId;
                    }
                }
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
                if (_proModel != null && !string.IsNullOrEmpty(_proModel.ldCate))
                {
                    var selectedCategory = _categoryNameToIdMap.FirstOrDefault(c => c.Value == _proModel.ldCate).Key;
                    if (selectedCategory != null)
                    {
                        SelectedCategory = selectedCategory;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An error occurred while loading categories: {ex.Message}");
            }
        }
    }
}