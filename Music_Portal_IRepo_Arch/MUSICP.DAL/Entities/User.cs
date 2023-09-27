namespace MusicPortal.DAL.Entities
{
    public class User
    {
        public int Id { get; set; }
        
        public string? Login { get; set; }

        public string? Pwd { get; set; }

        public string? Salt { get; set; }
        public string? Status { get; set; }

        public bool IsActivated { get; set; }
    }
}