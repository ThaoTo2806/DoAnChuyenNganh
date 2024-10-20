using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace QuanLyMaWinApp.Models
{
    public class Customer
    {
        public int idCustomer { get; set; }
        public string? Name { get; set; }
        public string? Image { get; set; }
        public int? IdUser { get; set; }
        public bool? IsDeleted { get; set; } = false;

    }
    public class Member
    {
        public int IdUser { get; set; }
        public string? Account { get; set; }
        public string? PassWord { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public bool? Gender { get; set; }
        public DateTime? Birth { get; set; }
        public int? IdRole { get; set; }
        public bool? IsDeleted { get; set; } = false;
        public string iconDelete { get; set; }
        public string iconEdit { get; set; }
        public Member ()
        {
            iconDelete = "delete.png";
            iconEdit = "edit.png";
        }

        public string GenderDisplay => Gender == true ? "Female" : Gender == false ? "Male" : "Unknown";

        public async Task<bool> DangNhapAsync(string account, string password)
        {
            using (var client = new HttpClient())
            {
                var url = "http://localhost:5134/api/Users/login"; // Thay <port> bằng cổng thực tế của API

                var loginRequest = new
                {
                    Account = account,
                    PassWord = password
                };

                var jsonContent = JsonSerializer.Serialize(loginRequest);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                try
                {
                    var response = await client.PostAsync(url, content);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseBody = await response.Content.ReadAsStringAsync();
                        var user = JsonSerializer.Deserialize<Member>(responseBody, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true,
                            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                        });

                        if (user != null && user.IdRole == 1 && user.IsDeleted == false)
                        {
                            return true;
                        }
                    }
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine($"Request error: {ex.Message}");
                }

                return false;
            }
        }


    }
}
