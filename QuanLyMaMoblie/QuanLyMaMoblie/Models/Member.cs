using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace QuanLyMaMoblie.Models
{
    public class Member
    {
        public int IdUser { get; set; }
        public string Account { get; set; }
        public string PassWord { get; set; }
        public int IdRole { get; set; } = 2;
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Image { get; set; }
        public bool? Gender { get; set; } // nullable
        public string Address { get; set; }
        public DateTime? Birth { get; set; }
        public bool? IsDeleted { get; set; } = false; // Set default value to false
        public bool isDelete { get; set; }
        public string GenderDisplay => Gender == true ? "Female" : Gender == false ? "Male" : "Unknown";
        public Member() { }

        private static readonly HttpClient _client = new HttpClient
        {
            Timeout = TimeSpan.FromMinutes(1) // Đặt thời gian chờ là 1 phút
        };

        public async Task<bool> DangNhapAsync(string account, string password)
        {
            var url = "http://192.168.1.23:5134/api/Users/login-regular"; // Sử dụng địa chỉ IP của máy tính

            var loginRequest = new
            {
                Account = account,
                PassWord = password
            };

            var jsonContent = JsonSerializer.Serialize(loginRequest);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            try
            {
                Console.WriteLine($"Sending POST request to {url} with content: {jsonContent}");

                var response = await _client.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    var user = JsonSerializer.Deserialize<Member>(responseBody, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    });

                    if (user != null && user.IdRole == 2 && user.IsDeleted == false)
                    {
                        return true;
                    }
                    else
                    {
                        Console.WriteLine($"User is either not a regular user or is deleted. IdType: {user?.IdRole}, IsDeleted: {user?.IsDeleted}");
                    }
                }
                else
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Login failed with status code {response.StatusCode}. Response: {responseBody}");
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Request error: {ex.Message}");

                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
            }
            catch (TaskCanceledException ex)
            {
                Console.WriteLine($"Request timeout: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }

            return false;
        }

        public async Task<bool> RegisterAsync(string name, string email, string account, string password, string phone)
        {
            var url = "http://192.168.1.23:5134/api/Users/register";

            var registerRequest = new
            {
                Name = name,
                Email = email,
                Phone = phone,
                Account = account,
                PassWord = password
            };

            var jsonContent = JsonSerializer.Serialize(registerRequest);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            try
            {
                Console.WriteLine($"Sending POST request to {url} with content: {jsonContent}");

                var response = await _client.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    var user = JsonSerializer.Deserialize<Member>(responseBody, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    });

                    return true; // Đăng ký thành công
                }
                else
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Register failed with status code {response.StatusCode}. Response: {responseBody}");
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Request error: {ex.Message}");

                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
            }
            catch (TaskCanceledException ex)
            {
                Console.WriteLine($"Request timeout: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }

            return false;
        }

        public async Task<Member> GetProfileAsync()
        {
            var url = "http://192.168.1.23:5134/api/Users/profile";

            try
            {
                Console.WriteLine($"Sending GET request to {url}");

                var response = await _client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    var user = JsonSerializer.Deserialize<Member>(responseBody, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    });

                    return user;
                }
                else
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Get profile failed with status code {response.StatusCode}. Response: {responseBody}");
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Request error: {ex.Message}");

                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
            }
            catch (TaskCanceledException ex)
            {
                Console.WriteLine($"Request timeout: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }

            return null;
        }

        public async Task<bool> LogoutAsync()
        {
            var url = "http://192.168.1.23:5134/api/Users/logout";

            try
            {
                Console.WriteLine($"Sending POST request to {url}");

                var response = await _client.PostAsync(url, null);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("User logged out successfully");
                    return true;
                }
                else
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Logout failed with status code {response.StatusCode}. Response: {responseBody}");
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Request error: {ex.Message}");

                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
            }
            catch (TaskCanceledException ex)
            {
                Console.WriteLine($"Request timeout: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }

            return false;
        }

        public async Task<bool> UpdateUserAsync(int id, string name, string email, string address, string image, bool gender, string phone, DateTime birth)
        {
            var updateUserRequest = new
            {
                Name = name,
                Email = email,
                Address = address,
                Phone = phone,
                Birth = birth,
                Gender = gender,
                Image = image
            };

            var url = "http://192.168.1.23:5134/api/Users/Update?id={id}";

            var jsonContent = JsonSerializer.Serialize(updateUserRequest);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            try
            {
                var response = await _client.PutAsync(url, content);
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error: {response.StatusCode}, {responseContent}");
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Request error: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
            }
            catch (TaskCanceledException ex)
            {
                Console.WriteLine($"Request timeout: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }

            return false;
        }

        public async Task<int> GetTotalQuantity()
        {
            var url = "http://192.168.1.23:5134/api/Cart/total-quantity";
            try
            {
                Console.WriteLine($"Sending GET request to {url}");

                if (string.IsNullOrEmpty(url))
                {
                    Console.WriteLine("The URL is null or empty.");
                    return 0;
                }

                var response = await _client.GetAsync(url);

                // Kiểm tra trạng thái của phản hồi
                Console.WriteLine($"Response status code: {response.StatusCode}");

                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Response: {responseBody}");

                    var totalQuantityResponse = JsonSerializer.Deserialize<CartTotalQuantity>(responseBody, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    });

                    return totalQuantityResponse.TotalQuantity;
                }
                else
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Failed to get total quantity. Status code: {response.StatusCode}. Response: {responseBody}");
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Request error: {ex.Message}");

                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
            }
            catch (TaskCanceledException ex)
            {
                Console.WriteLine($"Request timeout: {ex.Message}");
            }

            return 0;
        }

        public async Task<Cart> GetCartAsync()
        {
            var url = "http://192.168.1.23:5134/api/Cart";

            try
            {
                Console.WriteLine($"Sending GET request to {url}");

                var response = await _client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    var cart = JsonSerializer.Deserialize<Cart>(responseBody, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    });

                    return cart;
                }
                else
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Failed to get cart. Status code: {response.StatusCode}. Response: {responseBody}");
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Request error: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
            }
            catch (TaskCanceledException ex)
            {
                Console.WriteLine($"Request timeout: {ex.Message}");
            }

            return null;
        }

        public async Task<bool> IncrementQuantityAsync(string productId)
        {
            var url = "http://192.168.1.23:5134/api/Cart/increment";

            var request = new
            {
                ProductId = productId
            };

            var jsonContent = JsonSerializer.Serialize(request);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            try
            {
                Console.WriteLine($"Sending POST request to {url} with content: {jsonContent}");

                var response = await _client.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Product quantity incremented successfully");
                    return true;
                }
                else
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Increment failed with status code {response.StatusCode}. Response: {responseBody}");
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Request error: {ex.Message}");

                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
            }
            catch (TaskCanceledException ex)
            {
                Console.WriteLine($"Request timeout: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }

            return false;
        }
        public async Task<bool> DecrementQuantityAsync(string productId)
        {
            var url = "http://192.168.1.23:5134/api/Cart/decrement";

            var request = new
            {
                ProductId = productId
            };

            var jsonContent = JsonSerializer.Serialize(request);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            try
            {
                Console.WriteLine($"Sending POST request to {url} with content: {jsonContent}");

                var response = await _client.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Product quantity decremented successfully");
                    return true;
                }
                else
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Decrement failed with status code {response.StatusCode}. Response: {responseBody}");
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Request error: {ex.Message}");

                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
            }
            catch (TaskCanceledException ex)
            {
                Console.WriteLine($"Request timeout: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }

            return false;
        }

        public async Task<bool> RemoveFromCartAsync(string productId)
        {
            var url = "http://192.168.1.23:5134/api/Cart/remove";

            var request = new
            {
                ProductId = productId
            };

            var jsonContent = JsonSerializer.Serialize(request);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            try
            {
                Console.WriteLine($"Sending POST request to {url} with content: {jsonContent}");

                var response = await _client.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Product removed from cart successfully");
                    return true;
                }
                else
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Remove from cart failed with status code {response.StatusCode}. Response: {responseBody}");
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Request error: {ex.Message}");

                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
            }
            catch (TaskCanceledException ex)
            {
                Console.WriteLine($"Request timeout: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }

            return false;
        }
        public async Task<bool> AddToCartAsync(string productId, int quantity)
        {
            var url = "http://192.168.1.23:5134/api/Cart/add";

            var request = new
            {
                ProductId = productId,
                Quantity = quantity
            };

            var jsonContent = JsonSerializer.Serialize(request);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            try
            {
                Console.WriteLine($"Sending POST request to {url} with content: {jsonContent}");

                var response = await _client.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Product added to cart successfully");
                    return true;
                }
                else
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Add to cart failed with status code {response.StatusCode}. Response: {responseBody}");
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Request error: {ex.Message}");

                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
            }
            catch (TaskCanceledException ex)
            {
                Console.WriteLine($"Request timeout: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }

            return false;
        }
        public async Task<OrderInfo> GetOrderInfoAsync()
        {
            var url = "http://192.168.1.23:5134/api/Orders/order-info"; // Update the URL to match your API endpoint

            try
            {
                Console.WriteLine($"Sending GET request to {url}");

                var response = await _client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    var orderInfo = JsonSerializer.Deserialize<OrderInfo>(responseBody, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    });

                    return orderInfo;
                }
                else
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Get order info failed with status code {response.StatusCode}. Response: {responseBody}");
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Request error: {ex.Message}");

                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
            }
            catch (TaskCanceledException ex)
            {
                Console.WriteLine($"Request timeout: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }

            return null;
        }

        public async Task<CreateOrderResponse> CreateOrderAsync(string deliveryAddress, string paymentStatus)
        {
            var url = "http://192.168.1.23:5134/api/Orders/create-order";

            // Tạo đối tượng yêu cầu cho việc tạo đơn hàng
            var createOrderRequest = new CreateOrderRequest
            {
                DeliveryAddress = deliveryAddress,
                PaymentStatus = paymentStatus
            };

            var jsonContent = JsonSerializer.Serialize(createOrderRequest);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            try
            {
                Console.WriteLine($"Sending POST request to {url} with content: {jsonContent}");

                var response = await _client.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    var createOrderResponse = JsonSerializer.Deserialize<CreateOrderResponse>(responseBody, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    });

                    if (createOrderResponse != null)
                    {
                        Console.WriteLine($"Order created successfully. OrderId: {createOrderResponse.OrderId}, ActivationCodeId: {createOrderResponse.ActivationCodeId}");
                        return createOrderResponse;
                    }
                    else
                    {
                        Console.WriteLine("Failed to deserialize the response.");
                    }
                }
                else
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Create order failed with status code {response.StatusCode}. Response: {responseBody}");
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Request error: {ex.Message}");

                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
            }
            catch (TaskCanceledException ex)
            {
                Console.WriteLine($"Request timeout: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }

            return null;
        }
        public async Task<List<Order>> GetOrdersAsync()
        {
            var url = "http://192.168.1.23:5134/api/Orders";

            try
            {
                Console.WriteLine($"Send a GET request to {url}");

                var response = await _client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    var orders = JsonSerializer.Deserialize<List<Order>>(responseBody, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    });

                    return orders;
                }
                else
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Retrieve failed orders with status code {response.StatusCode}. Inform: {responseBody}");
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Request error: {ex.Message}");

                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Internal error: {ex.InnerException.Message}");
                }
            }
            catch (TaskCanceledException ex)
            {
                Console.WriteLine($"The request time expired: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }

            return null;
        }
        public async Task<List<ExpiredOrder>> GetExpritedCodeOrdersAsync()
        {
            var url = "http://192.168.1.23:5134/api/Orders/expired-code-orders";

            try
            {
                Console.WriteLine($"Send a GET request to {url}");

                var response = await _client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    var orders = JsonSerializer.Deserialize<List<ExpiredOrder>>(responseBody, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    });

                    return orders;
                }
                else
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Retrieve failed orders with status code {response.StatusCode}. Inform: {responseBody}");
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Request error: {ex.Message}");

                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Internal error: {ex.InnerException.Message}");
                }
            }
            catch (TaskCanceledException ex)
            {
                Console.WriteLine($"The request time expired: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }

            return null;
        }
        public async Task<UserOrderDetails> GetUserOrderDetailsAsync(int orderId)
        {
            var url = $"http://192.168.1.23:5134/api/Orders/userorder-details?orderId={orderId}";

            try
            {
                Console.WriteLine($"Sending GET request to {url}");

                var response = await _client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    var userOrderDetails = JsonSerializer.Deserialize<UserOrderDetails>(responseBody, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    });

                    return userOrderDetails;
                }
                else
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Get user order details failed with status code {response.StatusCode}. Response: {responseBody}");
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Request error: {ex.Message}");
            }
            catch (TaskCanceledException ex)
            {
                Console.WriteLine($"Request timeout: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }

            return null;
        }

        public async Task<bool> RequestActivationAsync(ActivationRequest activationRequest)
        {
            if (activationRequest == null)
            {
                Console.WriteLine("ActivationRequest cannot be null.");
                return false;
            }
            var url = "http://192.168.1.23:5134/api/Orders/request-activation";

            try
            {
                Console.WriteLine($"Sending PUT request to {url}");

                // Chuyển đổi đối tượng yêu cầu kích hoạt thành chuỗi JSON
                var jsonContent = JsonSerializer.Serialize(activationRequest);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                // Gửi yêu cầu PUT đến API
                var response = await _client.PutAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Request activation succeeded.");
                    return true;
                }
                else
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Request activation failed with status code {response.StatusCode}. Response: {responseBody}");
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Request error: {ex.Message}");

                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Internal error: {ex.InnerException.Message}");
                }
            }
            catch (TaskCanceledException ex)
            {
                Console.WriteLine($"Request timeout: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }

            return false;
        }

        public async Task<bool> ForgotPassAsync(UpdatePasswordRequest request)
        {
            var url = "http://192.168.1.23:5134/api/Users/update-password";

            // Chuyển đổi đối tượng request thành JSON
            var jsonContent = JsonSerializer.Serialize(request);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            try
            {
                // Gọi API
                var response = await _client.PutAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error: {response.StatusCode}, {responseContent}");
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Request error: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
            }
            catch (TaskCanceledException ex)
            {
                Console.WriteLine($"Request timeout: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }

            return false;
        }
    }
    public class Order : ObservableObject
    {
        private string _orderStatus;
        private Color _frameColor3;
        private Color _frameColor4;

        public int OrderId { get; set; }
        public string FirstImage { get; set; }
        public string FirstProductName { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime DeliveryDate { get; set; }

        public string OrderStatus
        {
            get => _orderStatus;
            set
            {
                SetProperty(ref _orderStatus, value);
                UpdateFrameColors();
            }
        }

        public Color FrameColor3
        {
            get => _frameColor3;
            set => SetProperty(ref _frameColor3, value);
        }

        public Color FrameColor4
        {
            get => _frameColor4;
            set => SetProperty(ref _frameColor4, value);
        }

        public void UpdateFrameColors()
        {
            switch (OrderStatus)
            {
                case "Processing":
                    FrameColor3 = Colors.Blue;
                    FrameColor4 = Colors.White;
                    break;
                case "Confirmed":
                    FrameColor3 = Colors.Orange;
                    FrameColor4 = Colors.Black;
                    break;
                case "Cancelled":
                    FrameColor3 = Colors.Red;
                    FrameColor4 = Colors.White;
                    break;
                case "Delivered":
                    FrameColor3 = Colors.Green;
                    FrameColor4 = Colors.White;
                    break;
                default:
                    FrameColor3 = Colors.Transparent;
                    FrameColor4 = Colors.Black;
                    break;
            }
        }
    }
    public class ExpiredOrder : ObservableObject
    {
        public int OrderId { get; set; }
        public string FirstImage { get; set; }
        public string FirstProductName { get; set; }
        public DateTime InitiatedDate { get; set; }
        public DateTime ExpiredDate { get; set; }

        public string CStatus { get; set; }
    }

    public class OrderInfo
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public int OrderId { get; set; }
        public List<CartItem> CartItems { get; set; }
        public double TotalPrice { get; set; }
        public string ActivationCodeInfo { get; set; }
    }


    public class CreateOrderRequest
    {
        public string DeliveryAddress { get; set; }
        public string PaymentStatus { get; set; }
    }

    public class CreateOrderResponse
    {
        public int OrderId { get; set; }
        public int ActivationCodeId { get; set; }
        public string Status { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public decimal Price { get; set; }
    }
    public class UserOrderDetails : ObservableObject
    {
        public OrderDetails Order { get; set; }
        public List<ProductDetails> Products { get; set; }
    }

    public class OrderDetails
    {
        public int OrderId { get; set; }
        public string UserName { get; set; }
        public string Phone { get; set; }
        public string DeliveryAddress { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public int TotalQuantity { get; set; }
        public decimal TotalPrice { get; set; }
        public string PaymentStatus { get; set; }
        public string OrderStatus { get; set; }
        public string ActiCode { get; set; }
        public string DinhKy { get; set; }
        public decimal? ActivationPrice { get; set; }
        public DateTime? NgayKhoiTao { get; set; }
        public DateTime? NgayHetHan { get; set; }
    }

    public class ProductDetails
    {
        public string ProductImage { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal ProductPrice { get; set; }
    }

}
