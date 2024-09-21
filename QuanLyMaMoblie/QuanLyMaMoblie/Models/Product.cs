using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Maui.Graphics;

namespace QuanLyMaMoblie.Models
{
    public partial class Product : ObservableObject
    {
        public ActivationCode? ActivationCode { get; set; }
        public string ID { get; set; }
        public string ProductName { get; set; }
        public string ldCate { get; set; }
        public int? ldCode { get; set; }
        public double Evaluate { get; set; }
        public string? Image { get; set; }
        public int SL { get; set; }
        public double Price { get; set; }
        public string? Detail { get; set; }
        public string? Feature { get; set; }
        public string? Specifications { get; set; }
        public string? Helps { get; set; }
        public string? Sitecode { get; set; }
        public string? MID { get; set; }
        public bool IsDeleted { get; set; }

        [ObservableProperty]
        private Color frameColor;

        public int QuantityInCart { get; set; } // Số lượng trong giỏ hàng

        [ObservableProperty]
        private int checkInCart; // Đánh giá sp vào giỏ hàng

        public Product()
        {
        }

        private static readonly HttpClient _httpClient = new HttpClient { BaseAddress = new Uri("http://192.168.1.48:5134/") };

        public static async Task<List<Product>> GetProductsAsync()
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<Product>>("api/Products");
                return response ?? new List<Product>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while fetching Products: {ex.Message}");
                return new List<Product>();
            }
        }
    }
}
