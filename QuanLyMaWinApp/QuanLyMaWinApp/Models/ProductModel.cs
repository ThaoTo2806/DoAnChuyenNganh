using System;
using System.Collections.Generic;
using System.Data;
using System.Net.Http.Json;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace QuanLyMaWinApp.Models
{
    public class ProductModel
    {
        public CategoryModel Category { get; set; }
        public string ID { get; set; }
        public string Name { get; set; }
        public string IdCate { get; set; }
        public double Evaluate { get; set; }
        public int SL { get; set; }
        public double Price { get; set; }
        public string? Detail { get; set; }
        public string? Feature { get; set; }
        public string? Specifications { get; set; }
        public string? Helps { get; set; }
        public int? IdVersion { get; set; }
        public bool IsDeleted { get; set; }
        public string? Image { get; set; }
        public string iconEdit { get; set; }
        public string iconDelete { get; set; }

        public ProductModel()
        {
            iconDelete = "delete.png";
            iconEdit = "edit.png";
        }

        private static readonly HttpClient _httpClient = new HttpClient { BaseAddress = new Uri("http://localhost:5134/") };

        public static async Task<List<ProductModel>> GetProductsAsync()
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<ProductModel>>("api/Products");
                return response ?? new List<ProductModel>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while fetching Products: {ex.Message}");
                return new List<ProductModel>();
            }
        }

        public static async Task<bool> SoftDeleteProductAsync(string id)
        {
            try
            {
                var response = await _httpClient.PutAsync($"api/Products/SoftDelete1/{id}", null);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while soft deleting the product: {ex.Message}");
                return false;
            }
        }

        public static async Task<bool> UpdateProductDetailsAsync(string id, string productName, string ldCate, string image, int sl, double price, string detail, string feature, string specifications, string helps, int idVersion)
        {
            var request = new ProductUpdateRequest
            {
                Name = productName,
                IdCate = ldCate,
                Image = image,
                SL = sl,
                Price = price,
                Detail = detail,
                Feature = feature,
                Specifications = specifications,
                Helps = helps,
                IdVersion = idVersion
            };

            try
            {
                var response = await _httpClient.PutAsJsonAsync($"api/Products/UpdateDetails1?id={id}", request);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while updating product details: {ex.Message}");
                return false;
            }
        }

        public static async Task<bool> InsertProductAsync(string id, string ProductName, string IdCate, string image, int SL, double Price, string detail, string ft, string sp, string hp, int version)
        {
            var request = new ProductInsertRequest
            {
                ID = id,
                Name = ProductName,
                IdCate = IdCate,
                Image = image,
                SL = SL,
                Price= Price,
                Detail = detail,
                Feature= ft,
                Specifications = sp,
                Helps = hp,
                IdVersion= version
            };

            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/Products/Insert1", request);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while inserting the Product: {ex.Message}");
                return false;
            }
        }

        public class ProductUpdateRequest
        {
            public string Name { get; set; }
            public string IdCate { get; set; }
            public string? Image { get; set; }
            public int SL { get; set; }
            public double Price { get; set; }
            public string? Detail { get; set; }
            public string? Feature { get; set; }
            public string? Specifications { get; set; }
            public string? Helps { get; set; }
            public int? IdVersion { get; set; }
        }

        public class ProductInsertRequest
        {
            public string ID { get; set; }
            public string Name { get; set; }
            public string IdCate { get; set; }
            public string? Image { get; set; }
            public int SL { get; set; }
            public double Price { get; set; }
            public string? Detail { get; set; }
            public string? Feature { get; set; }
            public string? Specifications { get; set; }
            public string? Helps { get; set; }
            public int? IdVersion { get; set; }

        }

    }
}
