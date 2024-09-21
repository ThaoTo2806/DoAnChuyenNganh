namespace webapi.Model
{
    public class OrderInformation
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public List<CartItem> CartItems { get; set; }
        public double TotalPrice { get; set; }
        public string ActivationCodeInfo { get; set; }
    }

}
