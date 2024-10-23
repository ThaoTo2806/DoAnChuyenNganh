using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using QuanLyMaWinApp.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Diagnostics;
using QuanLyMaWinApp.Views;
using CommunityToolkit.Mvvm.Input;

namespace QuanLyMaWinApp.ViewModels
{
    public class OrderViewModel : ObservableObject
    {
        private const int PageSize1 = 5;
        private int _currentPage1 = 1;
        private const int PageSize2 = 5;
        private int _currentPage2 = 1;
        private bool _isBusy1;
        private bool _isBusy2;
        private List<OrderModel> _allProcessingOrders = new List<OrderModel>();
        private List<OrderModel> _allConfirmedOrders = new List<OrderModel>();

        public bool IsBusy1
        {
            get => _isBusy1;
            set => SetProperty(ref _isBusy1, value);
        }

        public bool IsBusy2
        {
            get => _isBusy2;
            set => SetProperty(ref _isBusy2, value);
        }


        public ObservableCollection<OrderModel> PaidOrders { get; } = new ObservableCollection<OrderModel>();
        public ObservableCollection<OrderModel> UnPaidOrders { get; } = new ObservableCollection<OrderModel>();

        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }
        public IAsyncRelayCommand LoadOrders1Command { get; }
        public IAsyncRelayCommand LoadOrders2Command { get; }
        public ICommand LoadMore1Command { get; }
        public ICommand LoadMore2Command { get; }
        public ICommand NextPage1Command { get; }
        public ICommand NextPage2Command { get; }
        public ICommand PreviousPage1Command { get; }
        public ICommand PreviousPage2Command { get; }

        private bool _canGoToPreviousPage1;
        public bool CanGoToPreviousPage1
        {
            get => _canGoToPreviousPage1;
            private set => SetProperty(ref _canGoToPreviousPage1, value);
        }

        private bool _canGoToPreviousPage2;
        public bool CanGoToPreviousPage2
        {
            get => _canGoToPreviousPage2;
            private set => SetProperty(ref _canGoToPreviousPage2, value);
        }


        private bool _canGoToNextPage1;
        public bool CanGoToNextPage1
        {
            get => _canGoToNextPage1;
            private set => SetProperty(ref _canGoToNextPage1, value);
        }

        private bool _canGoToNextPage2;
        public bool CanGoToNextPage2
        {
            get => _canGoToNextPage2;
            private set => SetProperty(ref _canGoToNextPage2, value);
        }

        public string CurrentPageLabel1 => $"Page {_currentPage1}";
        public string CurrentPageLabel2 => $"Page {_currentPage2}";

        public OrderViewModel()
        {
            EditCommand = new Command<OrderModel>(async (order) => await OnEdit(order));
            DeleteCommand = new Command<OrderModel>(async (order) => await OnDelete(order));

            LoadOrders1Command = new AsyncRelayCommand(LoadOrders1Async);
            LoadOrders2Command = new AsyncRelayCommand(LoadOrders2Async);
            LoadMore1Command = new Command(async () => await LoadMoreOrders1Async());
            LoadMore2Command = new Command(async () => await LoadMoreOrders2Async());

            NextPage1Command = new Command(async () => await GoToNextPage1Async());
            PreviousPage1Command = new Command(async () => await GoToPreviousPage1Async());

            NextPage2Command = new Command(async () => await GoToNextPage2Async());
            PreviousPage2Command = new Command(async () => await GoToPreviousPage2Async());

            LoadOrders1Async();
            LoadOrders2Async();
        }

        private async Task LoadOrders1Async()
        {
            try
            {
                IsBusy1 = true;
                _currentPage1 = 1;
                _allProcessingOrders = await OrderModel.GetProcessingOrdersAsync();
                UnPaidOrders.Clear();
                foreach (var processingO in _allProcessingOrders.Take(PageSize1))
                {
                    UnPaidOrders.Add(processingO);
                }

                CanGoToNextPage1 = _allProcessingOrders.Count > PageSize1;
                CanGoToPreviousPage1 = false;
                OnPropertyChanged(nameof(CurrentPageLabel1));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while loading processing orders: {ex.Message}");
            }
            finally
            {
                IsBusy1 = false;
            }
        }

        private async Task LoadOrders2Async()
        {
            try
            {
                IsBusy2 = true;
                _currentPage2 = 1;
                _allConfirmedOrders = await OrderModel.GetConfirmedDeliveredOrdersAsync();
                PaidOrders.Clear();
                foreach (var confrimO in _allConfirmedOrders.Take(PageSize2))
                {
                    PaidOrders.Add(confrimO);
                }

                CanGoToNextPage2 = _allConfirmedOrders.Count > PageSize2;
                CanGoToPreviousPage2 = false;
                OnPropertyChanged(nameof(CurrentPageLabel2));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while loading processing orders: {ex.Message}");
            }
            finally
            {
                IsBusy2 = false;
            }
        }

        private async Task LoadMoreOrders1Async()
        {
            if (IsBusy1)
                return;

            try
            {
                IsBusy1 = true;
                var nextProOrders = _allProcessingOrders.Skip(_currentPage1 * PageSize1).Take(PageSize1);

                foreach (var pro in nextProOrders)
                {
                    UnPaidOrders.Add(pro);
                }

                CanGoToNextPage1 = nextProOrders.Any();
                _currentPage1++;
                CanGoToPreviousPage1 = _currentPage1 > 1;
                OnPropertyChanged(nameof(CurrentPageLabel1));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while loading more processing orders: {ex.Message}");
            }
            finally
            {
                IsBusy1 = false;
            }
        }

        private async Task LoadMoreOrders2Async()
        {
            if (IsBusy2)
                return;

            try
            {
                IsBusy2 = true;
                var nextProOrders = _allConfirmedOrders.Skip(_currentPage2 * PageSize2).Take(PageSize2);

                foreach (var pro in nextProOrders)
                {
                    PaidOrders.Add(pro);
                }

                CanGoToNextPage2 = nextProOrders.Any();
                _currentPage2++;
                CanGoToPreviousPage2 = _currentPage2 > 1;
                OnPropertyChanged(nameof(CurrentPageLabel2));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while loading more confirmed orders: {ex.Message}");
            }
            finally
            {
                IsBusy2 = false;
            }
        }

        private async Task GoToNextPage1Async()
        {
            if (!CanGoToNextPage1 || IsBusy1)
                return;

            try
            {
                IsBusy1 = true;
                _currentPage1++;
                var nextProOrders = _allProcessingOrders.Skip((_currentPage1 - 1) * PageSize1).Take(PageSize1).ToList();

                UnPaidOrders.Clear();
                foreach (var pro in nextProOrders)
                {
                    UnPaidOrders.Add(pro);
                }

                CanGoToNextPage1 = _currentPage1 * PageSize1 < _allProcessingOrders.Count;
                CanGoToPreviousPage1 = _currentPage1 > 1;
                OnPropertyChanged(nameof(CurrentPageLabel1));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while loading the next page: {ex.Message}");
            }
            finally
            {
                IsBusy1 = false;
            }
        }

        private async Task GoToNextPage2Async()
        {
            if (!CanGoToNextPage2 || IsBusy2)
                return;

            try
            {
                IsBusy2 = true;
                _currentPage2++;
                var nextProOrders = _allConfirmedOrders.Skip((_currentPage2 - 1) * PageSize2).Take(PageSize2).ToList();

                PaidOrders.Clear();
                foreach (var pro in nextProOrders)
                {
                    PaidOrders.Add(pro);
                }

                CanGoToNextPage2 = _currentPage2 * PageSize2 < _allConfirmedOrders.Count;
                CanGoToPreviousPage2 = _currentPage2 > 1;
                OnPropertyChanged(nameof(CurrentPageLabel2));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while loading the next page: {ex.Message}");
            }
            finally
            {
                IsBusy2 = false;
            }
        }

        private async Task GoToPreviousPage1Async()
        {
            if (!CanGoToPreviousPage1 || IsBusy1)
                return;

            try
            {
                IsBusy1 = true;
                _currentPage1--;
                var previousProOrders = _allProcessingOrders.Skip((_currentPage1 - 1) * PageSize1).Take(PageSize1);

                UnPaidOrders.Clear();
                foreach (var pro in previousProOrders)
                {
                    UnPaidOrders.Add(pro);
                }

                CanGoToNextPage1 = _currentPage1 * PageSize1 < _allProcessingOrders.Count;
                CanGoToPreviousPage1 = _currentPage1 > 1;
                OnPropertyChanged(nameof(CurrentPageLabel1));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while loading the previous page: {ex.Message}");
            }
            finally
            {
                IsBusy1 = false;
            }
        }

        private async Task GoToPreviousPage2Async()
        {
            if (!CanGoToPreviousPage2 || IsBusy2)
                return;

            try
            {
                IsBusy2 = true;
                _currentPage2--;
                var previousProOrders = _allConfirmedOrders.Skip((_currentPage2 - 1) * PageSize2).Take(PageSize2);

                PaidOrders.Clear();
                foreach (var pro in previousProOrders)
                {
                    PaidOrders.Add(pro);
                }

                CanGoToNextPage2 = _currentPage2 * PageSize2 < _allConfirmedOrders.Count;
                CanGoToPreviousPage2 = _currentPage2 > 1;
                OnPropertyChanged(nameof(CurrentPageLabel2));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while loading the previous page: {ex.Message}");
            }
            finally
            {
                IsBusy2 = false;
            }
        }

        private async Task OnEdit(OrderModel order)
        {
            try
            {
                if (order != null)
                {
                    var editOrderPage = new EditOrderPage(order.OrderId);
                    editOrderPage.Disappearing += async (s, e) =>
                    {
                        await LoadOrders1Async();
                        await LoadOrders2Async();
                    };
                    await Application.Current.MainPage.Navigation.PushAsync(editOrderPage);
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


        private async Task OnDelete(OrderModel order)
        {
            try
            {
                if (order != null)
                {
                    Debug.WriteLine($"Attempting to delete: {order.OrderId}");

                    bool result = await Device.InvokeOnMainThreadAsync(() =>
                    {
                        return Application.Current.MainPage.DisplayAlert("Confirm Delete", $"Are you sure you want to delete order {order.OrderId}?", "Yes", "No");
                    });

                    if (result)
                    {
                        string note = await Application.Current.MainPage.DisplayPromptAsync(
                            "Input Note",
                            "Please enter the reason for deletion or any additional notes:",
                            accept: "OK",
                            cancel: "Cancel"
                        );

                        if (!string.IsNullOrWhiteSpace(note))
                        {
                            if (await OrderModel.UpdateOrder1Async(order.OrderId, note))
                            {
                                await Application.Current.MainPage.DisplayAlert("Success", "Order deleted successfully", "OK");
                                UnPaidOrders.Remove(order);
                            }
                            else
                            {
                                await Application.Current.MainPage.DisplayAlert("Error", "Could not delete the order.", "OK");
                            }
                        }
                        else
                        {
                            await Application.Current.MainPage.DisplayAlert("Error", "You must provide a note to delete the order.", "OK");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error delete category: {ex.Message}");
            }
        }

    }
}
