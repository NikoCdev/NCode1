namespace Gbook.Models
{
    public class Message
    {
        public int Id { get; set; }

        public  User User { get; set; }
        public string MessageText { get; set; }
        public DateTime MessageDate { get; set; }
        
    }
}
