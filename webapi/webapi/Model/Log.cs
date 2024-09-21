namespace webapi.Model
{
    public class Log
    {
        public int ID { get; set; }
        public int? IdMember { get; set; }
        public DateTime? Activity { get; set; }
        public string Detail { get; set; }

        public User User { get; set; }
    }
}
