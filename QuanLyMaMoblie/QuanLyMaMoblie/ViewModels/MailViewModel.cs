using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using QuanLyMaMoblie.Models;
using QuanLyMaMoblie.Views;

namespace QuanLyMaMoblie.ViewModels
{
    public partial class MailViewModel : ObservableObject
    {
        private List<ExpiredOrder> _expiredOrdersList;

        public List<ExpiredOrder> ExpiredOrdersList
        {
            get => _expiredOrdersList;
            set => SetProperty(ref _expiredOrdersList, value);
        }

        public Member _member;
        public ICommand LoadOrdersCommand { get; }
        public ICommand HomeCommand { get; }
        public ICommand NoteCommand { get; }
        public ICommand MailCommand { get; }
        public ICommand UserCommand { get; }
        public ICommand GoBackCommand { get; }
        public ICommand NavigateToDetailOrderCommand { get; }
        public MailViewModel()
        {
            LoadOrdersCommand = new AsyncRelayCommand(LoadOrdersAsync);
            HomeCommand = new Command(async () => await NavigateToHome());
            NoteCommand = new Command(async () => await NavigateToNote());
            MailCommand = new Command(async () => await NavigateToMail());
            UserCommand = new Command(async () => await NavigateToUser());
            GoBackCommand = new Command(async () => await GoBackAsync());
            LoadOrdersCommand.Execute(null);
            NavigateToDetailOrderCommand = new Command(NavigateToDetailOrderAsync);
        }

        private async void NavigateToDetailOrderAsync()
        {
            try
            {
                var activationView = new ActivationView(ExpiredOrdersList);
                await Application.Current.MainPage.Navigation.PushAsync(activationView);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error navigating to ActivationView: {ex.Message}");
            }
        }

        private async Task GoBackAsync()
        {
            // Navigate to IndexView
            await Application.Current.MainPage.Navigation.PushAsync(new IndexView());
        }

        private async Task LoadOrdersAsync()
        {
            try
            {
                _member = new Member();
                await Task.Delay(2000);
                ExpiredOrdersList = await _member.GetExpritedCodeOrdersAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading data: {ex.Message}");
            }
        }

        private async Task NavigateToHome()
        {
            try
            {
                await Application.Current.MainPage.Navigation.PushAsync(new IndexView());
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error navigating to Home: {ex.Message}");
            }
        }

        private async Task NavigateToNote()
        {
            try
            {
                await Application.Current.MainPage.Navigation.PushAsync(new AllOrdersView());
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error navigating to Note: {ex.Message}");
            }
        }

        private async Task NavigateToMail()
        {
            try
            {
                await Application.Current.MainPage.Navigation.PushAsync(new MailView());
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error navigating to Mail: {ex.Message}");
            }
        }

        private async Task NavigateToUser()
        {
            try
            {
                await Application.Current.MainPage.Navigation.PushAsync(new UserView());
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error navigating to User: {ex.Message}");
            }
        }
    }

}
