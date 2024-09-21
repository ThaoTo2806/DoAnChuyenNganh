namespace webapi.Model
{
    public class CartItem
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string image {  get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public double TotalPrice => Price * Quantity;
    }

    public class Cart
    {
        public List<CartItem> Items { get; set; } = new List<CartItem>();

        public int TotalQuantity => Items.Sum(item => item.Quantity);
        public double TotalPrice => Items.Sum(item => item.TotalPrice);
    }

    public class CartAddRequest
    {
        public string ProductId { get; set; }
        public int Quantity { get; set; }
    }

    public class CartUpdateRequest
    {
        public string ProductId { get; set; }
        public int Quantity { get; set; }
    }

    public class CartRemoveRequest
    {
        public string ProductId { get; set; }
    }

}
