namespace webapi.Model
{
    public class User
    {
        public int ID { get; set; }
        public string Account { get; set; }
        public string PassWord { get; set; }
        public bool? IdType { get; set; } = false; // nullable
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public string? Image { get; set; }
        public bool? Gender { get; set; }
        public string? Address { get; set; }
        public bool? IsDeleted { get; set; } = false; // Set default value to false
        public string? Email { get; set; }
        public DateTime? Birth { get; set; }
    }
}
