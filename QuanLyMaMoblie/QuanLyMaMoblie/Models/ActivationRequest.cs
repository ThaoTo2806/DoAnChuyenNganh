using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyMaMoblie.Models
{
    public class ActivationRequest
    {
        public int OrderID { get; set; }
        public string Title { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PayMent { get; set; }
        public DateTime RequestDay { get; set; }
        public int Periodic { get; set; }
        public double Total { get; set; }
    }

}
