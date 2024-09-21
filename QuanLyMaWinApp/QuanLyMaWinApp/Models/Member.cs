using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace QuanLyMaWinApp.Models
{
    public class Member
    {
        public int ID { get; set; }
        public string Account { get; set; }
        public string PassWord { get; set; }
        public bool? IdType { get; set; } = false;
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Image { get; set; }
        public bool? Gender { get; set; } // nullable
        public string Address { get; set; }
        public DateTime Birth { get; set; }
        public bool? IsDeleted { get; set; } = false; // Set default value to false
        public bool isDelete { get; set; }
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

                        if (user != null && user.IdType == true && user.IsDeleted == false)
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

        public static async Task<List<Member>> GetRegularUsersAsync()
        {
            using (var client = new HttpClient())
            {
                var url = "http://localhost:5134/api/Users/regular-users";

                try
                {
                    var response = await client.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseBody = await response.Content.ReadAsStringAsync();
                        var users = JsonSerializer.Deserialize<List<Member>>(responseBody, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true,
                            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                        });

                        return users ?? new List<Member>();
                    }
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine($"Request error: {ex.Message}");
                }

                return new List<Member>();
            }
        }

        public static async Task<bool> InsertMemberAsync(string name, string account, string phone, string image, bool gender, string address, string email, DateTime Birth)
        {
            using (var client = new HttpClient())
            {
                var url = "http://localhost:5134/api/Users/Insert";
                var request = new UserInsertRequest
                {
                    Name = name,
                    Account = account,
                    //PassWord = password,
                    Phone = phone,
                    Image = image,
                    Gender = gender,
                    Address = address,
                    Email = email,
                    Birth = Birth
                };

                try
                {
                    var response = await client.PostAsJsonAsync(url, request);
                    return response.IsSuccessStatusCode;
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine($"Request error: {ex.Message}");
                    return false;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred while inserting the User: {ex.Message}");
                    return false;
                }
            }
        }

        public class UserInsertRequest
        {
            public string Name { get; set; }
            public string Account { get; set; }
            //public string PassWord { get; set; }
            public string Phone { get; set; }
            public string Image { get; set; }
            public bool Gender { get; set; }
            public string Address { get; set; }
            public string Email { get; set; }
            public DateTime Birth { get; set; }
        }

    }
}
