namespace webapi.Model
{
    public class User
    {
        public int IdUser { get; set; }
        public string? Account { get; set; }
        public string? Username { get; set; }
        public string? PassWord { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public bool? Gender { get; set; }
        public DateTime? Birth { get; set; }
        public int? IdRole { get; set; }
        public bool? IsDeleted { get; set; } = false;

    }
}
