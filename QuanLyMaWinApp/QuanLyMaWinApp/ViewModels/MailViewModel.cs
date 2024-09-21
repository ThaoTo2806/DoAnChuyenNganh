using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using QuanLyMaWinApp.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using QuanLyMaWinApp.Views;
using CommunityToolkit.Mvvm.Input;
using Org.BouncyCastle.Asn1.Ocsp;

namespace QuanLyMaWinApp.ViewModels
{
    public class MailViewModel : ObservableObject
    {
        private const int PageSize = 5;
        private int _currentPage = 1;
        private bool _isBusy;
        private List<ActivationRequest> _allRequests = new List<ActivationRequest>();
        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }
        public ObservableCollection<ActivationRequest> Requests { get; set; } = new ObservableCollection<ActivationRequest>();
        public ICommand SeeCommand { get; }
        public IAsyncRelayCommand LoadRequestsCommand { get; }
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


        public MailViewModel()
        {
            SeeCommand = new Command<ActivationRequest>(async (mail) => await OnSee(mail));
            LoadRequestsCommand = new AsyncRelayCommand(LoadRequestsAsync);
            LoadMoreCommand = new Command(async () => await LoadMoreRequestsAsync());

            NextPageCommand = new Command(async () => await GoToNextPageAsync());
            PreviousPageCommand = new Command(async () => await GoToPreviousPageAsync());

            LoadRequestsAsync();
        }

        private async Task LoadRequestsAsync()
        {
            try
            {
                IsBusy = true;
                _currentPage = 1;
                _allRequests = await ActivationRequest.GetRequestsAsync();
                Requests.Clear();
                foreach (var request in _allRequests.Take(PageSize))
                {
                    Requests.Add(request);
                }

                CanGoToNextPage = _allRequests.Count > PageSize;
                CanGoToPreviousPage = false; // Reset to false since we are on the first page
                OnPropertyChanged(nameof(CurrentPageLabel));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while loading requests: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }
        private async Task LoadMoreRequestsAsync()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;
                var nextRequests = _allRequests.Skip(_currentPage * PageSize).Take(PageSize);

                foreach (var request in nextRequests)
                {
                    Requests.Add(request);
                }

                CanGoToNextPage = nextRequests.Any();
                _currentPage++;
                CanGoToPreviousPage = _currentPage > 1;
                OnPropertyChanged(nameof(CurrentPageLabel));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while loading more Requests: {ex.Message}");
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
                var nextRequests = _allRequests.Skip((_currentPage - 1) * PageSize).Take(PageSize).ToList();

                Requests.Clear();
                foreach (var request in nextRequests)
                {
                    Requests.Add(request);
                }

                CanGoToNextPage = _currentPage * PageSize < _allRequests.Count;
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
                var previousRequests = _allRequests.Skip((_currentPage - 1) * PageSize).Take(PageSize);

                Requests.Clear();
                foreach (var request in previousRequests)
                {
                    Requests.Add(request);
                }

                CanGoToNextPage = _currentPage * PageSize < _allRequests.Count;
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
        public async Task OnSee(ActivationRequest mail)
        {
            try
            {

                if (mail != null)
                {
                    var seeCodePage = new SendCodePage(mail);
                    seeCodePage.Disappearing += async (s, e) =>
                    {
                        await LoadRequestsAsync();
                    };
                    await Application.Current.MainPage.Navigation.PushAsync(seeCodePage);
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Error", "Cannot proceed to the next page", "OK");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error! An error occurred. Please try again later: {ex.Message}");
            }
        }
    }
}
