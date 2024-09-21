using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace QuanLyMaWinApp.Models
{
    public class Log
    {
        public int ID { get; set; }
        public int? IdMember { get; set; }
        public DateTime? Activity { get; set; }
        public string Detail { get; set; }

        public Log() { }
        private static readonly HttpClient _httpClient = new HttpClient { BaseAddress = new Uri("http://localhost:5134/") };

        public static async Task<List<Log>> GetLogsAsync()
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<Log>>("api/Users/logs");
                return response ?? new List<Log>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while fetching Logs: {ex.Message}");
                return new List<Log>();
            }
        }
    }

}
