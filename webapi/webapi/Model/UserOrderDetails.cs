namespace webapi.Model
{
    public class UserOrderDetails
    {
        public OrderDetails Order { get; set; }
        public List<ProductDetails> Products { get; set; }
    }
}
