namespace webapi.Model
{
    public class ProductUpdateRequest
    {
        public string ProductName { get; set; }
        public string ldCate { get; set; }
        public string? Image { get; set; }
        public int SL { get; set; }
        public double Price { get; set; }
        public string? Detail { get; set; }
        public string? Feature { get; set; }
        public string? Specifications { get; set; }
        public string? Helps { get; set; }
    }
}
