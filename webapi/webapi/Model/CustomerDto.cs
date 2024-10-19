namespace webapi.Model
{
    public class CustomerDto
    {
        public int IdCustomer { get; set; }
        public string? Name { get; set; }
        public string? Image { get; set; }
        public bool? IsDeleted { get; set; }
        public int IdUser { get; set; }
        public DateTime? Birth { get; set; }
        public bool? Gender { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
    }
}
