namespace webapi.Model
{
    public class User_Customer
    {
        public int IdUser { get; set; }
        public string Account { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public bool Gender { get; set; }
        public DateTime Birth { get; set; }
        public int IdRole { get; set; }
        public bool IsDeletedUser { get; set; }
        public int IdCustomer { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public bool IsDeletedCustomer { get; set; }
    }
}
