using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyMaWinApp.Models
{
    public class ProductDetail
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
        public string Version { get; set; }
        public string Description { get; set; }
        public double VersionPrice { get; set; }
        public CategoryModel Category { get; set; }
        public string iconEdit { get; set; } = "edit.png";
        public string iconDelete { get; set; } = "delete.png";
    }
}
