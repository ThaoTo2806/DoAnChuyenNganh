﻿namespace webapi.Model
{
    public class UserInsertRequest
    {
        public string Name { get; set; }
        public string Account { get; set; }
        public string Phone { get; set; }
        public string Image { get; set; }
        public bool Gender { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public DateTime Birth { get; set; }
    }
}
