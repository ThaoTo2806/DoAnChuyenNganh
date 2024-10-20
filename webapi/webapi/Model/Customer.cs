﻿using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace webapi.Model
{
    public class Customer
    {
        public int idCustomer { get; set; }
        public string? Name { get; set; }
        public string? Image { get; set; }
        public int? IdUser { get; set; }
        public bool? IsDeleted { get; set; } = false;

        //public User? User { get; set; }
    }
}