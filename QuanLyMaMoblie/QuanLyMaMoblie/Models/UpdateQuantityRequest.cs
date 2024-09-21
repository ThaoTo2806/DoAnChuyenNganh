using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyMaMoblie.Models
{
    public class UpdateQuantityRequest
    {
        public string ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
