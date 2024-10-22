namespace webapi.Model
{
    public class Product
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string IdCate { get; set; }
        public double? Evaluate { get; set; }
        public int SL { get; set; }
        public double Price { get; set; }
        public string? Detail { get; set; }  
        public string? Feature { get; set; }  
        public string? Specifications { get; set; }  
        public string? Helps { get; set; } 
        public int? IdVersion { get; set; }
        public bool IsDeleted { get; set; }
        public string? Image { get; set; }

        // Navigation properties
        public Category Category { get; set; }
    }

}
