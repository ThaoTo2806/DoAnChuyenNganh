using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using QuanLyMaMoblie.Models;
using QuanLyMaMoblie.Views;
using System.Collections.ObjectModel;

namespace QuanLyMaMoblie.ViewModels
{
    public class InfoUserViewModel : ObservableObject
    {
        public Member _member { get; }

        public int ID
        {
            get => _member?.ID ?? 0; // Trả về 0 nếu _member là null
            set
            {
                if (_member != null && _member.ID != value)
                {
                    _member.ID = value; // Gán giá trị cho thuộc tính ID
                    OnPropertyChanged();
                }
            }
        }

        public string Name
        {
            get => _member?.Name;
            set
            {
                if (_member != null && _member.Name != value)
                {
                    _member.Name = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime Birth
        {
            get => (DateTime)(_member?.Birth);
            set
            {
                if (_member != null && _member.Birth != value)
                {
                    _member.Birth = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Email
        {
            get => _member?.Email;
            set
            {
                if (_member != null && _member.Email != value)
                {
                    _member.Email = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Phone
        {
            get => _member?.Phone;
            set
            {
                if (_member != null && _member.Phone != value)
                {
                    _member.Phone = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Address
        {
            get => _member?.Address;
            set
            {
                if (_member != null && _member.Address != value)
                {
                    _member.Address = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsMale
        {
            get => _member?.Gender == true;
            set
            {
                if (_member != null && value != (_member.Gender == true))
                {
                    _member.Gender = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(IsFemale)); // Thông báo thay đổi cho IsFemale
                }
            }
        }

        public bool IsFemale
        {
            get => _member?.Gender == false;
            set
            {
                if (_member != null && value != (_member.Gender == false))
                {
                    _member.Gender = !value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(IsMale)); // Thông báo thay đổi cho IsMale
                }
            }
        }

        public string Image
        {
            get => _member?.Image;
            set
            {
                if (_member != null && _member.Image != value)
                {
                    _member.Image = value;
                    OnPropertyChanged();
                }
            }
        }

        private ImageSource _selectedImage;

        public ImageSource SelectedImage
        {
            get => _selectedImage;
            set => SetProperty(ref _selectedImage, value);
        }

        public ICommand SaveCommand { get; }
        public ICommand NoteCommand { get; }
        public ICommand ChooseImageCommand => new AsyncRelayCommand(OnChooseImageClicked);

        public InfoUserViewModel()
        {
            SaveCommand = new AsyncRelayCommand(SaveExecute);
            NoteCommand = new AsyncRelayCommand(NavigateToNote);
        }

        public InfoUserViewModel(Member member) : this()
        {
            _member = member;
            NoteCommand = new AsyncRelayCommand(NavigateToNote);
        }

        private async Task NavigateToNote()
        {
            try
            {
                await Application.Current.MainPage.Navigation.PushAsync(new UserView());
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error navigating to Note: {ex.Message}");
            }
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

                    _member.Image = imagePath;
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
                bool success = await _member.UpdateUserAsync(
                    ID,
                    Name,
                    Email,
                    Address,
                    Image,
                    gender,
                    Phone,
                    Birth
                );

                if (success)
                {
                    await Application.Current.MainPage.DisplayAlert("Success", "Information updated successfully", "OK");

                    if (Application.Current.MainPage.Navigation.NavigationStack.Count > 1)
                    {
                        await Application.Current.MainPage.Navigation.PopAsync();
                    }
                }
                else
                {

                    await Application.Current.MainPage.DisplayAlert("Error", "Could not update the information", "OK");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An error occurred while updating the information: {ex.Message}");
                await Application.Current.MainPage.DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
        }
    }
}
