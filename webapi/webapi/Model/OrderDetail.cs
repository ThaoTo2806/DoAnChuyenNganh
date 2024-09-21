namespace webapi.Model
{
    public class OrderDetail
    {
        public int ID { get; set; } // Định danh chi tiết đơn hàng
        public string OrderId { get; set; } // ID của đơn hàng
        public string ProductId { get; set; } // ID của sản phẩm
        public int Quantity { get; set; } // Số lượng sản phẩm
        public decimal UnitPrice { get; set; } // Giá đơn vị của sản phẩm
    }
}
