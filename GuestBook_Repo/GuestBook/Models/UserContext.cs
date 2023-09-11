using Microsoft.EntityFrameworkCore;

namespace Gbook.Models
{
    public class UserContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Message> Messages { get; set; }

        public UserContext(DbContextOptions<UserContext> options)
            : base(options)
        {
            Database.EnsureCreated();
            if (!Users.Any()) 
            {
                var user1 = new User
                {
                    Login = "User1",
                    Pwd = "password1" 
                };

                var user2 = new User
                {
                    Login = "User2",
                    Pwd = "password2" 
                };

                Users.Add(user1);
                Users.Add(user2);

                SaveChanges();

                var message1 = new Message
                {
                    User = user1,
                    MessageText = "Привет, User1!",
                    MessageDate = DateTime.Now
                };

                var message2 = new Message
                {
                    User = user2,
                    MessageText = "Привет, User2!",
                    MessageDate = DateTime.Now
                };

                Messages.Add(message1);
                Messages.Add(message2);

                SaveChanges();
            }
        }


      
    }
}