using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace QuanLyMaWinApp.Models
{
    public class CategoryModel
    {
        public string ID { get; set; }
        public string CategoryName { get; set; }
        public string Detail { get; set; }
        public string iconEdit { get; set; }
        public string iconDelete { get; set; }
        public bool isDelete { get; set; }

        public CategoryModel()
        {
            iconDelete = "delete.png";
            iconEdit = "edit.png";
        }

        private static readonly HttpClient _httpClient = new HttpClient { BaseAddress = new Uri("http://localhost:5134/") };

        public static async Task<List<CategoryModel>> GetCategoriesAsync()
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<CategoryModel>>("api/Categories");
                return response ?? new List<CategoryModel>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while fetching categories: {ex.Message}");
                return new List<CategoryModel>();
            }
        }


        public static async Task<bool> UpdateCategoryDetailsAsync(string id, string categoryName, string detail)
        {
            var request = new CategoryUpdateRequest
            {
                CategoryName = categoryName,
                Detail = detail
            };

            try
            {
                var response = await _httpClient.PutAsJsonAsync($"api/Categories/UpdateDetails?id={id}", request);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while updating category details: {ex.Message}");
                return false;
            }
        }

        public static async Task<bool> SoftDeleteCategoryAsync(string id)
        {
            try
            {
                var response = await _httpClient.PutAsync($"api/Categories/SoftDelete/{id}", null);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while soft deleting the category: {ex.Message}");
                return false;
            }
        }


        public static async Task<bool> InsertCategoryAsync(string id, string categoryName, string detail)
        {
            var request = new CategoryInsertRequest
            {
                ID = id,
                CategoryName = categoryName,
                Detail = detail
            };

            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/Categories/Insert", request);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while inserting the category: {ex.Message}");
                return false;
            }
        }

        public class CategoryInsertRequest
        {
            public string ID { get; set; }
            public string CategoryName { get; set; }
            public string Detail { get; set; }
        }

        public class CategoryUpdateRequest
        {
            public string CategoryName { get; set; }
            public string Detail { get; set; }
        }
    }
}
