﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace QuanLyMaWinApp.Models
{
    public class OrderModel
    {
        public string iconEdit { get; set; }
        public string iconDelete { get; set; }

        public int OrderId { get; set; }
        public string DCGH { get; set; }
        public DateTime NgayDat { get; set; }
        public DateTime? NgayGiao { get; set; }
        public string ThanhToanStatus { get; set; }
        public string DonHangStatus { get; set; }
        public int TotalQuantity { get; set; }
        public decimal TotalPrice { get; set; }

        public OrderModel()
        {
            iconDelete = "delete.png";
            iconEdit = "edit.png";
        }

        private static readonly HttpClient _httpClient = new HttpClient { BaseAddress = new Uri("http://localhost:5134/") };

        public static async Task<List<OrderModel>> GetProcessingOrdersAsync()
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<OrderModel>>("api/Orders/processing-orders");
                return response ?? new List<OrderModel>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while fetching processing orders: {ex.Message}");
                return new List<OrderModel>();
            }
        }

        public static async Task<List<OrderModel>> GetConfirmedDeliveredOrdersAsync()
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<OrderModel>>("api/Orders/confirmed-delivered-orders");
                return response ?? new List<OrderModel>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while fetching confirmed-delivered orders: {ex.Message}");
                return new List<OrderModel>();
            }
        }

        public static async Task<OrderDetailModel> GetOrderDetailsByIdAsync(int orderId)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<OrderDetailModel>($"api/Orders/order-details/{orderId}");
                return response ?? new OrderDetailModel();
            }
            catch (JsonException jsonEx)
            {
                Console.WriteLine($"JSON conversion error: {jsonEx.Message}");
                return new OrderDetailModel();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while fetching order details: {ex.Message}");
                return new OrderDetailModel();
            }
        }

        public static async Task<bool> UpdateOrderAsync(int id) 
        {
            var request = new OrderUpdateRequest
            {
                Status = "Confirmed",
                note = "Confirmed"
            };

            try
            {
                var response = await _httpClient.PutAsJsonAsync($"api/Orders/UpdateOrders?id={id}", request);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while updating order status: {ex.Message}");
                return false;
            }
        }

        public static async Task<bool> UpdateOrder1Async(int id, string note)
        {
            var request = new OrderUpdateRequest
            {
                Status = "Cancelled",
                note = note
            };

            try
            {
                var response = await _httpClient.PutAsJsonAsync($"api/Orders/UpdateOrders?id={id}", request);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while updating order status: {ex.Message}");
                return false;
            }
        }
        public class OrderUpdateRequest
        {
            public string Status { get; set; }
            public string? note { get; set; }
        }
    }
}
