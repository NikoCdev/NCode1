using Microsoft.EntityFrameworkCore;
using MusicPortal.DAL.Entities;
using MusicPortal.DAL.Interfaces;
using MusicPortal.DAL.EF;


namespace MusicPortal.DAL.Repositories
{
    public class SongRepository : IRepository<Song>
    {
        private UserContext db;

        public SongRepository(UserContext context)
        {
            this.db = context;
        }

        public async Task<IEnumerable<Song>> GetAll()
        {
            return await db.Songs.ToListAsync();
        }

        public async Task<Song> Get(int id)
        {
            var players = await db.Songs.Where(a => a.Id == id).ToListAsync();
            Song? player = players?.FirstOrDefault();
            return player;
        }

        public async Task<Song> Get(string name)
        {         
            var players = await db.Songs.Where(a => a.Title == name).ToListAsync();
            Song? player = players?.FirstOrDefault();
            return player;
        }

        public async Task Create(Song player)
        {
            await db.Songs.AddAsync(player);
        }

        public void Update(Song player)
        {
            db.Entry(player).State = EntityState.Modified;
        }

        public async Task Delete(int id)
        {
            Song? player = await db.Songs.FindAsync(id);
            if (player != null)
                db.Songs.Remove(player);
        }

    }
}
