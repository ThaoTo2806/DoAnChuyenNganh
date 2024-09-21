namespace webapi.Model
{
    public class Product
    {
        public string ID { get; set; }
        public string ProductName { get; set; }
        public string ldCate { get; set; }
        //public int? ldCode { get; set; }
        public double Evaluate { get; set; }
        public string? Image { get; set; }
        public int SL { get; set; }
        public double Price { get; set; }
        public string? Detail { get; set; }  
        public string? Feature { get; set; }  
        public string? Specifications { get; set; }  
        public string? Helps { get; set; } 
        public string? Sitecode { get; set; }  
        public string? MID { get; set; }  
        public bool IsDeleted { get; set; }

        // Navigation properties
        public Category Category { get; set; }
        //public ActivationCode? ActivationCode { get; set; }
    }

}
