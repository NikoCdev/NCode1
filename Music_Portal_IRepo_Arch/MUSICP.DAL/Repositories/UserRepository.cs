using Microsoft.EntityFrameworkCore;
using MusicPortal.DAL.Entities;
using MusicPortal.DAL.Interfaces;
using MusicPortal.DAL.EF;


namespace MusicPortal.DAL.Repositories
{
    public class UserRepository : IRepository<User>
    {
        private UserContext db;

        public UserRepository(UserContext context)
        {
            this.db = context;
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await db.UsersE.ToListAsync();
        }

        public async Task<User> Get(int id)
        {
            var users = await db.UsersE.Where(a => a.Id == id).ToListAsync();
            User? user = users?.FirstOrDefault();
            return user;
        }

        public async Task<User> Get(string login)
        {         
            var users = await db.UsersE.Where(a => a.Login == login).ToListAsync();
            User? user = users?.FirstOrDefault();
            return user;
        }

        public async Task Create(User user)
        {
            await db.UsersE.AddAsync(user);
        }

        public void Update(User user)
        {
            db.Entry(user).State = EntityState.Modified;
        }

        public async Task Delete(int id)
        {
            User? user = await db.UsersE.FindAsync(id);
            if (user != null)
                db.UsersE.Remove(user);
        }

    }
}
