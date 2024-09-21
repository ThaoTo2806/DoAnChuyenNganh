namespace webapi.Model
{
    public class UserUpdateRequest
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Image { get; set; }
        public bool? Gender { get; set; } // nullable
        public string Address { get; set; }
        public string Email { get; set; }
        public DateTime Birth { get; set; }
    }
}
