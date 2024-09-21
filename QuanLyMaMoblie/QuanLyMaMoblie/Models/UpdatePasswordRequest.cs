using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyMaMoblie.Models
{
    public class UpdatePasswordRequest
    {
        public string Account { get; set; }
        public string Email { get; set; }
    }
}
