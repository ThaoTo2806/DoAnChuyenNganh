using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace QuanLyMaWinApp.Models
{
    public class ActivationRequest
    {
        public int OrderID { get; set; }
        public string Title { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PayMent { get; set; }
        public DateTime RequestDay { get; set; }
        public int Periodic { get; set; }
        public double Total { get; set; }
        public string iconSee { get; set; }

        public ActivationRequest()
        {
            iconSee = "eye.png";
        }

        private static readonly HttpClient _httpClient = new HttpClient { BaseAddress = new Uri("http://localhost:5134/") };

        public static async Task<List<ActivationRequest>> GetRequestsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/Orders/activation-requests");
                response.EnsureSuccessStatusCode(); // Ensure we get a successful response status code

                var result = await response.Content.ReadFromJsonAsync<List<ActivationRequest>>();
                return result ?? new List<ActivationRequest>();
            }
            catch (HttpRequestException httpEx)
            {
                Console.WriteLine($"HTTP request error: {httpEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while fetching requests: {ex.Message}");
            }

            return new List<ActivationRequest>();
        }

        public static async Task<bool> CreateNewCodeAsync(int id, int dk, string email)
        {
            var request = new UpdateActiceCode
            {
                IdOrder = id,
                dinhKy = dk,
                Email = email
            };

            try
            {
                var response = await _httpClient.PostAsJsonAsync("/api/Orders/update-activationcode", request);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while create new activation: {ex.Message}");
                return false;
            }
        }
    }
}
