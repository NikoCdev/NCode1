using Microsoft.EntityFrameworkCore;
using MusicPortal.DAL.Entities;
using MusicPortal.DAL.Interfaces;
using MusicPortal.DAL.EF;


namespace MusicPortal.DAL.Repositories
{
    public class GenreRepository : IRepository<Genre>
    {
        private UserContext db;

        public GenreRepository(UserContext context)
        {
            this.db = context;
        }

        public async Task<IEnumerable<Genre>> GetAll()
        {
            return await db.Genres.ToListAsync();
        }

        public async Task<Genre> Get(int id)
        {
            var genres = await db.Genres.Include(o => o.Id).Where(a => a.Id == id).ToListAsync();
            Genre? genre = genres?.FirstOrDefault();
            return genre;
        }

        public async Task<Genre> Get(string name)
        {         
            var genres = await db.Genres.Where(a => a.Name == name).ToListAsync();
            Genre? genre = genres?.FirstOrDefault();
            return genre;
        }

        public async Task Create(Genre genre)
        {
            await db.Genres.AddAsync(genre);
        }

        public void Update(Genre genre)
        {
            db.Entry(genre).State = EntityState.Modified;
        }

        public async Task Delete(int id)
        {
            Genre? genre = await db.Genres.FindAsync(id);
            if (genre != null)
                db.Genres.Remove(genre);
        }

    }
}
