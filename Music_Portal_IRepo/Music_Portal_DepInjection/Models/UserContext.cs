using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace Gbook.Models
{
    public class UserContext : DbContext
    {
        public DbSet<User> UsersE { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Song> Songs { get; set; }

        public UserContext(DbContextOptions<UserContext> options)
            : base(options)
        {
            Database.EnsureCreated();


            if (!UsersE.Any()) 
            {
                byte[] saltbuf = new byte[16];
                RandomNumberGenerator randomNumberGenerator = RandomNumberGenerator.Create();
                randomNumberGenerator.GetBytes(saltbuf);
                StringBuilder sb = new StringBuilder(16);
                for (int i = 0; i < 16; i++)
                    sb.Append(string.Format("{0:X2}", saltbuf[i]));
                string salt = sb.ToString();               
                byte[] password = Encoding.Unicode.GetBytes(salt + "password1");            
                var md5 = MD5.Create();              
                byte[] byteHash = md5.ComputeHash(password);
                StringBuilder hash = new StringBuilder(byteHash.Length);
                for (int i = 0; i < byteHash.Length; i++)
                    hash.Append(string.Format("{0:X2}", byteHash[i]));

                string password_ = hash.ToString();
                string salt_ = salt;

                var user1 = new User
                {
                    Login = "User1",
                    Pwd = password_,
                    Status = "User",
                    Salt = salt_,
                    IsActivated = true


                };

                saltbuf = new byte[16];
                randomNumberGenerator = RandomNumberGenerator.Create();
                randomNumberGenerator.GetBytes(saltbuf);
                sb = new StringBuilder(16);
                for (int i = 0; i < 16; i++)
                    sb.Append(string.Format("{0:X2}", saltbuf[i]));
                salt = sb.ToString();
                password = Encoding.Unicode.GetBytes(salt + "password2");
                md5 = MD5.Create();
                byteHash = md5.ComputeHash(password);
                hash = new StringBuilder(byteHash.Length);
                for (int i = 0; i < byteHash.Length; i++)
                    hash.Append(string.Format("{0:X2}", byteHash[i]));

                password_ = hash.ToString();
                salt_ = salt;

                var user2 = new User
                {
                    Login = "User2",
                    Pwd = password_,
                    Status = "User",
                    Salt = salt_,
                    IsActivated = true
                };

                saltbuf = new byte[16];
                randomNumberGenerator = RandomNumberGenerator.Create();
                randomNumberGenerator.GetBytes(saltbuf);
                sb = new StringBuilder(16);
                for (int i = 0; i < 16; i++)
                    sb.Append(string.Format("{0:X2}", saltbuf[i]));
                salt = sb.ToString();
                password = Encoding.Unicode.GetBytes(salt + "passwordAdmin");
                md5 = MD5.Create();
                byteHash = md5.ComputeHash(password);
                hash = new StringBuilder(byteHash.Length);
                for (int i = 0; i < byteHash.Length; i++)
                    hash.Append(string.Format("{0:X2}", byteHash[i]));

                password_ = hash.ToString();
                salt_ = salt;

                var CommonAdmin = new User
                {
                    Login = "CommonAdmin",
                    Pwd = password_,
                    Status = "Admin",
                    Salt = salt_,
                    IsActivated = true
                };

                UsersE.Add(user1);
                UsersE.Add(user2);
                UsersE.Add(CommonAdmin);

                var genres = new List<Genre>
                {
                    new Genre { Name = "Rock" },
                    new Genre { Name = "Pop" },
                    new Genre { Name = "Hip Hop" },
                    
                };
                Genres.AddRange(genres);
                SaveChanges();

               
                SaveChanges();

            }
        }


      
    }
}