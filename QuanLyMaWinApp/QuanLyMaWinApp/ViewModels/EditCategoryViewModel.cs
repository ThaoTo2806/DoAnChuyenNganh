using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using QuanLyMaWinApp.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace QuanLyMaWinApp.ViewModels
{
    public class EditCategoryViewModel : ObservableObject
    {
        private CategoryModel _ctgModel;

        public string ID
        {
            get => _ctgModel?.ID;
            set
            {
                if (_ctgModel != null && _ctgModel.ID != value)
                {
                    _ctgModel.ID = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Name
        {
            get => _ctgModel?.CategoryName;
            set
            {
                if (_ctgModel != null && _ctgModel.CategoryName != value)
                {
                    _ctgModel.CategoryName = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Detail
        {
            get => _ctgModel?.Detail;
            set
            {
                if (_ctgModel != null && _ctgModel.Detail != value)
                {
                    _ctgModel.Detail = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand SaveCommand { get; }

        public EditCategoryViewModel()
        {
            SaveCommand = new Command(async () => await SaveExecute());
        }

        public EditCategoryViewModel(CategoryModel ctgModel) : this()
        {
            _ctgModel = ctgModel;
        }

        private async Task SaveExecute()
        {
            if (_ctgModel != null)
            {
                try
                {
                    // Call UpdateCategoryDetailsAsync to update the category
                    bool success = await CategoryModel.UpdateCategoryDetailsAsync(ID, Name, Detail);

                    if (success)
                    {
                        await Application.Current.MainPage.DisplayAlert("Success", "Category updated successfully", "OK");

                        // Navigate back to the previous page
                        if (Application.Current.MainPage.Navigation.NavigationStack.Count > 1)
                        {
                            await Application.Current.MainPage.Navigation.PopAsync();
                        }
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Error", "Could not update the category", "OK");
                    }
                }
                catch (Exception ex)
                {
                    // Log the error (if logging is set up)
                    Console.WriteLine($"An error occurred while updating the category: {ex.Message}");

                    // Show an alert to the user
                    await Application.Current.MainPage.DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
                }
            }
        }
    }
}
