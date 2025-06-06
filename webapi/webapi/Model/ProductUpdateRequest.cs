﻿namespace webapi.Model
{
    public class ProductUpdateRequest
    {
        public string Name { get; set; }
        public string IdCate { get; set; }
        public string? Image { get; set; }
        public int SL { get; set; }
        public double Price { get; set; }
        public string? Detail { get; set; }
        public string? Feature { get; set; }
        public string? Specifications { get; set; }
        public string? Helps { get; set; }
        public int? IdVersion { get; set; }
    }
}
