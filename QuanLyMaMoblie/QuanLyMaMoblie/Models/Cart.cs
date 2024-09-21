using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace QuanLyMaMoblie.Models
{
    public class Cart
    {
        public List<CartItem> Items { get; set; } = new List<CartItem>();
        public int TotalQuantity => Items.Sum(item => item.Quantity);
        public double TotalPrice => Items.Sum(item => item.TotalPrice);
    }

}
