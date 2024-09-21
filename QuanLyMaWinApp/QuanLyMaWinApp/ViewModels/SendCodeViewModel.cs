using System;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using QuanLyMaWinApp.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace QuanLyMaWinApp.ViewModels
{
    public class SendCodeViewModel : ObservableObject
    {
        public ActivationRequest _mailModel { get; }
        public ICommand SaveCommand { get; }

        public SendCodeViewModel()
        {
            SaveCommand = new Command(async () => await SaveExecute());
        }

        public SendCodeViewModel(ActivationRequest mailModel) : this()
        {
            _mailModel = mailModel;
        }

        private async Task SaveExecute()
        {
            try
            {
                // Retrieve parameters from _mailModel
                int orderId = _mailModel.OrderID;
                int periodic = _mailModel.Periodic;
                string email = _mailModel.Email;

                bool success = await ActivationRequest.CreateNewCodeAsync(orderId, periodic, email);

                if (success)
                {
                    await Application.Current.MainPage.DisplayAlert("Success", "Activation code created successfully", "OK");
                    if (Application.Current.MainPage.Navigation.NavigationStack.Count > 1)
                    {
                        await Application.Current.MainPage.Navigation.PopAsync();
                    }
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Failed to create activation code", "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Failed to save data: {ex.Message}", "OK");
            }
        }

    }
}
