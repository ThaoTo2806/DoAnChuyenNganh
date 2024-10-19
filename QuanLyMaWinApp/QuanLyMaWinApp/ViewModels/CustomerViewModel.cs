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
    public class CustomerViewModel : ObservableObject
    {
        private const int PageSize = 5;
        private int _currentPage = 1;
        private bool _isBusy;
        private List<CustomerDto> _allCustomers = new List<CustomerDto>();

        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        public ObservableCollection<CustomerDto> Customers { get; set; } = new ObservableCollection<CustomerDto>();

        public ICommand AddCommand { get; }
        public IAsyncRelayCommand LoadCustomersCommand { get; }
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

        public CustomerViewModel()
        {
            AddCommand = new Command(async () => await OnAddAsync());
            LoadCustomersCommand = new AsyncRelayCommand(LoadCustomersAsync);
            LoadMoreCommand = new Command(async () => await LoadMoreCustomersAsync());

            NextPageCommand = new Command(async () => await GoToNextPageAsync());
            PreviousPageCommand = new Command(async () => await GoToPreviousPageAsync());

            LoadCustomersAsync();
        }

        private async Task LoadCustomersAsync()
        {
            try
            {
                IsBusy = true;
                _currentPage = 1;
                _allCustomers = await CustomerDto.GetRegularUsersAsync();

                Console.WriteLine($"Loaded {_allCustomers.Count} customers."); // Thêm dòng này

                Customers.Clear();
                foreach (var customer in _allCustomers.Take(PageSize))
                {
                    Customers.Add(customer);
                }

                CanGoToNextPage = _allCustomers.Count > PageSize;
                CanGoToPreviousPage = false; // Reset to false since we are on the first page
                OnPropertyChanged(nameof(CurrentPageLabel));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while loading customers: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }


        private async Task LoadMoreCustomersAsync()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;
                var nextCustomers = _allCustomers.Skip(_currentPage * PageSize).Take(PageSize);

                foreach (var customer in nextCustomers)
                {
                    Customers.Add(customer);
                }

                CanGoToNextPage = nextCustomers.Any();
                _currentPage++;
                CanGoToPreviousPage = _currentPage > 1;
                OnPropertyChanged(nameof(CurrentPageLabel));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while loading more customers: {ex.Message}");
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
                var nextCustomers = _allCustomers.Skip((_currentPage - 1) * PageSize).Take(PageSize).ToList();

                Customers.Clear();
                foreach (var customer in nextCustomers)
                {
                    Customers.Add(customer);
                }

                CanGoToNextPage = _currentPage * PageSize < _allCustomers.Count;
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
                var previousCustomers = _allCustomers.Skip((_currentPage - 1) * PageSize).Take(PageSize);

                Customers.Clear();
                foreach (var customer in previousCustomers)
                {
                    Customers.Add(customer);
                }

                CanGoToNextPage = _currentPage * PageSize < _allCustomers.Count;
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

        private async Task OnAddAsync()
        {
            try
            {
                var addCustomerPage = new AddCustomerPage();
                addCustomerPage.Disappearing += async (s, e) =>
                {
                    await LoadCustomersAsync();
                };
                await Application.Current.MainPage.Navigation.PushAsync(addCustomerPage);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error when redirecting to AddCustomer page: {ex.Message}");
                await Application.Current.MainPage.DisplayAlert("Error", "Unable to move to add Customer page.", "OK");
            }
        }

        public async Task<string> GetGenderDisplayAsync(CustomerDto member)
        {
            try
            {
                //return member.GenderDisplay;
                return member.GenderDisplay;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while getting gender display: {ex.Message}");
                return "Unknown";
            }
        }

    }
}
