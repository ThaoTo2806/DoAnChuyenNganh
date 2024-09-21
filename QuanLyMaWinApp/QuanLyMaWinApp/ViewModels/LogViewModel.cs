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
    public class LogViewModel : ObservableObject
    {
        private const int PageSize = 20;
        private int _currentPage = 1;
        private bool _isBusy;
        private List<Log> _allLogs = new List<Log>();

        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        public ObservableCollection<Log> Logs { get; set; } = new ObservableCollection<Log>();

        public IAsyncRelayCommand LoadLogsCommand { get; }
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

        public LogViewModel()
        {
            LoadLogsCommand = new AsyncRelayCommand(LoadLogsAsync);
            LoadMoreCommand = new Command(async () => await LoadMoreLogsAsync());

            NextPageCommand = new Command(async () => await GoToNextPageAsync());
            PreviousPageCommand = new Command(async () => await GoToPreviousPageAsync());

            LoadLogsAsync();
        }

        private async Task LoadLogsAsync()
        {
            try
            {
                IsBusy = true;
                _currentPage = 1;
                _allLogs = await Log.GetLogsAsync();
                Logs.Clear();
                foreach (var log in _allLogs.Take(PageSize))
                {
                    Logs.Add(log);
                }

                CanGoToNextPage = _allLogs.Count > PageSize;
                CanGoToPreviousPage = false; // Reset to false since we are on the first page
                OnPropertyChanged(nameof(CurrentPageLabel));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while loading Logs: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task LoadMoreLogsAsync()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;
                var nextLogs = _allLogs.Skip(_currentPage * PageSize).Take(PageSize);

                foreach (var log in nextLogs)
                {
                    Logs.Add(log);
                }

                CanGoToNextPage = nextLogs.Any();
                _currentPage++;
                CanGoToPreviousPage = _currentPage > 1;
                OnPropertyChanged(nameof(CurrentPageLabel));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while loading more Logs: {ex.Message}");
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
                var nextLogs = _allLogs.Skip((_currentPage - 1) * PageSize).Take(PageSize).ToList();

                Logs.Clear();
                foreach (var log in nextLogs)
                {
                    Logs.Add(log);
                }

                CanGoToNextPage = _currentPage * PageSize < _allLogs.Count;
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
                var previousLogs = _allLogs.Skip((_currentPage - 1) * PageSize).Take(PageSize);

                Logs.Clear();
                foreach (var log in previousLogs)
                {
                    Logs.Add(log);
                }

                CanGoToNextPage = _currentPage * PageSize < _allLogs.Count;
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
    }
}
