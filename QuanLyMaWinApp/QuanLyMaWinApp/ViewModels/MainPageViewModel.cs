using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Maui.Controls;
using QuanLyMaWinApp.Models;
using QuanLyMaWinApp.Views;

namespace QuanLyMaWinApp.ViewModels
{
    public class MainPageViewModel : ObservableObject
    {
        private object _currentContent;

        public object CurrentContent
        {
            get { return _currentContent; }
            set { SetProperty(ref _currentContent, value); }
        }

        public ObservableCollection<Tool> Tools { get; } = new ObservableCollection<Tool>();

        public ICommand ItemSelectedCommand { get; }

        public MainPageViewModel()
        {
            InitializeTools();

            ItemSelectedCommand = new Command<Tool>(async (tool) => await NavigateToPageAsync(tool));
        }

        private void InitializeTools()
        {
            // Khởi tạo danh sách Tools với các mục tùy chọn
            Tools.Add(new Tool { Name = "Home", Image = "icons8home32.png" });
            Tools.Add(new Tool { Name = "Statistics", Image = "icons8chart32.png" });
            Tools.Add(new Tool { Name = "Category", Image = "icons8diversity32.png" });
            Tools.Add(new Tool { Name = "Product", Image = "icons8product32.png" });
            Tools.Add(new Tool { Name = "Customer", Image = "icons8customer32.png" });
            Tools.Add(new Tool { Name = "MailBox", Image = "icons8mailbox32.png" });
            Tools.Add(new Tool { Name = "Order", Image = "icons8order32.png" });
            Tools.Add(new Tool { Name = "Log", Image = "icons8log32.png" });
            Tools.Add(new Tool { Name = "Authorization", Image = "icons8admin32.png" });
            Tools.Add(new Tool { Name = "Log Out", Image = "icons8about32.png" });
            // Thêm các mục khác tương tự ở đây
        }

        private async Task NavigateToPageAsync(Tool tool)
        {
            try
            {
                switch (tool.Name)
                {
                    case "Home":
                        CurrentContent = new IndexView(); // Thiết lập CurrentContent để hiển thị IndexView1
                        break;
                    case "Statistics":
                        CurrentContent = new Statistics();
                        break;
                    case "Category":
                        CurrentContent = new Category();
                        break;
                    case "Customer":
                        CurrentContent = new CustomerView();
                        break;
                    case "Product":
                        CurrentContent = new ProductView();
                        break;
                    case "MailBox":
                        CurrentContent = new MailView();
                        break;
                    case "Order":
                        CurrentContent = new OrderView();
                        break;
                    case "Log":
                        CurrentContent = new LogView();
                        break;
                    case "Log Out":
                        bool answer = await App.Current.MainPage.DisplayAlert("Log Out", "Do you want to log out?", "Yes", "No");
                        if (answer)
                        {
                            var loginView = new LoginView();
                            await Application.Current.MainPage.Navigation.PushAsync(loginView);
                        }
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error when redirecting to page: {ex.Message}");
                await App.Current.MainPage.DisplayAlert("Error", "An error occurred while navigating to the page.", "OK");
            }
        }
    }
}
