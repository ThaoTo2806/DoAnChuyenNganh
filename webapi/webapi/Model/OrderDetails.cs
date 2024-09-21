namespace webapi.Model
{
    public class OrderDetails
    {
        public int OrderId { get; set; }
        public string UserName { get; set; }
        public string Phone { get; set; }
        public string DeliveryAddress { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public int TotalQuantity { get; set; }
        public decimal TotalPrice { get; set; }
        public string PaymentStatus { get; set; }
        public string OrderStatus { get; set; }
        public string ActiCode { get; set; }
        public string DinhKy { get; set; }
        public decimal? ActivationPrice { get; set; }
        public DateTime? NgayKhoiTao { get; set; }
        public DateTime? NgayHetHan { get; set; }
    }
}
