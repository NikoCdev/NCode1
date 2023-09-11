namespace Gbook.Models
{
    public class User
    {
        public int Id { get; set; }
        ICollection<Message>? Messages { get; set; }
        public string? Login { get; set; }

        public string? Pwd { get; set; }

        public string? Salt { get; set; }
    }
}