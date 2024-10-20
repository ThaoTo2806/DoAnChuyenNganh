using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace QuanLyMaWinApp.Models
{
    public class UserInsertRequest
    {
        public string Name { get; set; }
        public string Account { get; set; }
        public string Phone { get; set; }
        public string Image { get; set; }
        public bool Gender { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public DateTime Birth { get; set; }

        public UserInsertRequest() { }

        public static async Task<bool> InsertMemberAsync(string name, string account, string phone, string image, bool gender, string address, string email, DateTime Birth)
        {
            using (var client = new HttpClient())
            {
                var url = "http://localhost:5134/api/Users/Insert";
                var request = new UserInsertRequest
                {
                    Name = name,
                    Account = account,
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
    }
}
