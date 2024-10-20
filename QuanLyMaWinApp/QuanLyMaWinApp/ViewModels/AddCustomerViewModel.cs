using System;
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
    public class AddCustomerViewModel : ObservableObject
    {
        private UserInsertRequest _customer;

        public string Name
        {
            get => _customer?.Name;
            set
            {
                if (_customer != null && _customer.Name != value)
                {
                    _customer.Name = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Account
        {
            get => _customer?.Account;
            set
            {
                if (_customer != null && _customer.Account != value)
                {
                    _customer.Account = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime Birth
        {
            get => _customer?.Birth ?? DateTime.Now;
            set
            {
                if (_customer != null && _customer.Birth != value)
                {
                    _customer.Birth = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Phone
        {
            get => _customer?.Phone;
            set
            {
                if (_customer != null && _customer.Phone != value)
                {
                    _customer.Phone = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Email
        {
            get => _customer?.Email;
            set
            {
                if (_customer != null && _customer.Email != value)
                {
                    _customer.Email = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Address
        {
            get => _customer?.Address;
            set
            {
                if (_customer != null && _customer.Address != value)
                {
                    _customer.Address = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsMale
        {
            get => _customer?.Gender == true;
            set
            {
                if (_customer != null && value != (_customer.Gender == true))
                {
                    _customer.Gender = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(IsFemale)); // Thông báo thay đổi cho IsFemale
                }
            }
        }

        public bool IsFemale
        {
            get => _customer?.Gender == false;
            set
            {
                if (_customer != null && value != (_customer.Gender == false))
                {
                    _customer.Gender = !value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(IsMale)); // Thông báo thay đổi cho IsMale
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

        public AddCustomerViewModel()
        {
            SaveCommand = new AsyncRelayCommand(SaveExecute);
            _customer = new UserInsertRequest();
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

                    _customer.Image = imagePath;
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
                bool gender = IsMale;

                bool success = await UserInsertRequest.InsertMemberAsync(
                    Name,
                    Account,
                    Phone,
                    _customer.Image,
                    gender,
                    Address,
                    Email,
                    Birth
                );

                if (success)
                {
                    await Application.Current.MainPage.DisplayAlert("Success", "User created successfully", "OK");

                    if (Application.Current.MainPage.Navigation.NavigationStack.Count > 1)
                    {
                        await Application.Current.MainPage.Navigation.PopAsync();
                    }
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Could not create the User", "OK");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An error occurred while updating the product: {ex.Message}");
                await Application.Current.MainPage.DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
        }
    }
}