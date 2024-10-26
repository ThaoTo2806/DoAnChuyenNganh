namespace webapi.Model
{
    public class OrderDetailModel
    {
        public int OrderID { get; set; }
        public string AccCus { get; set; } 
        public string? UserName { get; set; }
        public string? UserEmail { get; set; }
        public string? UserPhone { get; set; }
        public string? UserAddress { get; set; }
        public string? Products { get; set; }
        public string? SLSP { get; set; }
        public string? GSP { get; set; }
        public string? Versions { get; set; }
        public decimal TotalVersionPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public string? OrderStatus { get; set; }
    }
}
