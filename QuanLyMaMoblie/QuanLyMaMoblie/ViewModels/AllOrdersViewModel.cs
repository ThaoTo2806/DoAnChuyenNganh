using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using QuanLyMaMoblie.Models;
using QuanLyMaMoblie.Views;

namespace QuanLyMaMoblie.ViewModels
{
    public partial class AllOrdersViewModel : ObservableObject
    {
        private List<Order> _ordersList;

        public List<Order> OrdersList
        {
            get => _ordersList;
            set => SetProperty(ref _ordersList, value);
        }

        public Member _member;

        public ICommand LoadOrdersCommand { get; }
        public ICommand HomeCommand { get; }
        public ICommand NoteCommand { get; }
        public ICommand MailCommand { get; }
        public ICommand UserCommand { get; }
        public ICommand GoBackCommand { get; }
        public ICommand NavigateToDetailOrderCommand { get; }

        public AllOrdersViewModel()
        {
            LoadOrdersCommand = new AsyncRelayCommand(LoadOrdersAsync);
            HomeCommand = new Command(async () => await NavigateToHome());
            NoteCommand = new Command(async () => await NavigateToNote());
            MailCommand = new Command(async () => await NavigateToMail());
            UserCommand = new Command(async () => await NavigateToUser());
            GoBackCommand = new Command(async () => await GoBackAsync());
            NavigateToDetailOrderCommand = new Command<Order>(NavigateToDetailOrderAsync);

            // Automatically load orders on initialization
            LoadOrdersCommand.Execute(null);
        }

        private async void NavigateToDetailOrderAsync(Order order)
        {
            try
            {
                if (order != null)
                {
                    // Chuyển tham số order.OrderId
                    await Application.Current.MainPage.Navigation.PushAsync(new DetailOrderView(order.OrderId));
                }
                else
                {
                    Debug.WriteLine("Order is null.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error navigating to DetailOrderView: {ex.Message}");
            }
        }

        private async Task GoBackAsync()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new IndexView());
        }

        private async Task LoadOrdersAsync()
        {
            try
            {
                _member = new Member();
                await Task.Delay(2000);
                var orders = await _member.GetOrdersAsync();

                foreach (var order in orders)
                {
                    order.UpdateFrameColors();
                }

                OrdersList = orders;
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
