using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyMaWinApp.Models
{
    public class OrderDetailModel
    {
        public int OrderID { get; set; }
        public string AccCus { get; set; } // Tài khoản khách hàng
        public string? UserName { get; set; } // Tên người dùng
        public string? UserEmail { get; set; } // Email người dùng
        public string? UserPhone { get; set; } // Số điện thoại
        public string? UserAddress { get; set; } // Địa chỉ
        public string? Products { get; set; } // Sản phẩm (nullable)
        public string? SLSP { get; set; } // Số lượng sản phẩm (nullable)
        public string? GSP { get; set; } // Giá sản phẩm (nullable)
        public string? Versions { get; set; } // Phiên bản sản phẩm (nullable)
        public decimal? TotalVersionPrice { get; set; } // Tổng giá phiên bản (nullable)
        public decimal TotalPrice { get; set; } // Tổng tiền
        public string? OrderStatus { get; set; } // Trạng thái đơn hàng
    }

}
