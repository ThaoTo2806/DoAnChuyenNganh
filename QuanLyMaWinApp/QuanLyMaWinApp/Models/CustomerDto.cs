using System;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace QuanLyMaWinApp.Models
{
    public class CustomerDto
    {
        public int IdCustomer { get; set; }
        public string? Name { get; set; }
        public string? Image { get; set; }
        public bool? IsDeleted { get; set; } = false;
        public int IdUser { get; set; }
        public DateTime? Birth { get; set; }
        public bool? Gender { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string iconDelete { get; set; }
        public string iconEdit { get; set; }
        public CustomerDto()
        {
            iconDelete = "delete.png";
            iconEdit = "edit.png";
        }

        public string GenderDisplay => Gender == true ? "Female" : Gender == false ? "Male" : "Unknown";

        public static async Task<List<CustomerDto>> GetRegularUsersAsync()
        {
            using (var client = new HttpClient())
            {
                var url = "http://localhost:5134/api/Users/regular-customers";

                try
                {
                    var response = await client.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseBody = await response.Content.ReadAsStringAsync();
                        var users = JsonSerializer.Deserialize<List<CustomerDto>>(responseBody, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true,
                            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                        });

                        return users ?? new List<CustomerDto>();
                    }
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine($"Request error: {ex.Message}");
                }

                return new List<CustomerDto>();
            }
        }
    }

}
