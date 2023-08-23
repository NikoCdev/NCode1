using Microsoft.EntityFrameworkCore;
using System.IO;

namespace MovieApp.Models
{
    // Чтобы подключиться к базе данных через Entity Framework, необходим контекст данных. 
    // Контекст данных представляет собой класс, производный от класса DbContext.
    public class ApplicationContext : DbContext
    {
        public DbSet<MovieModel> Movies { get; set; }
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
           : base(options)
        {
            Database.EnsureCreated();
        }
    }
}