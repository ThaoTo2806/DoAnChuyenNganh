namespace webapi.Model
{
    public class Order
    {
        public int ID { get; set; } // Định danh đơn hàng
        public string? CustomerId { get; set; } // ID của khách hàng, có thể null
        public DateTime? OrderDate { get; set; } // Ngày đặt hàng, có thể null
        public decimal? TotalAmount { get; set; } // Tổng số tiền đơn hàng, có thể null
        public string? Status { get; set; } // Trạng thái của đơn hàng, có thể null
        public List<OrderDetail>? OrderDetails { get; set; } // Các chi tiết của đơn hàng, có thể null
    }
}
