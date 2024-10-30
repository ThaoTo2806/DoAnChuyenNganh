using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;

namespace QuanLyMaMoblie.Models
{
    public class CartItem : ObservableObject
    {
        private int _quantity;

        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string Image { get; set; }
        public double Price { get; set; }
        public string tenVersion { get; set; }
        public double giaVersion { get; set; }
        public int Quantity
        {
            get => _quantity;
            set => SetProperty(ref _quantity, value);
        }
        public double TotalPrice => ((Price * Quantity) + (giaVersion * Quantity));
    }
}
