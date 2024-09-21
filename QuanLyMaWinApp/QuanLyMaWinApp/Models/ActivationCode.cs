using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace QuanLyMaWinApp.Models
{
    public class ActivationCode
    {
        public int ID { get; set; }
        public string? ActiCode { get; set; }
        public string? Status { get; set; }
        public DateTime NgayKhoiTao { get; set; }
        public DateTime NgayHetHan { get; set; }
        public DateTime NgayCapNhat { get; set; }
        public int DinhKy { get; set; }
        public double Price { get; set; }
    }
}
