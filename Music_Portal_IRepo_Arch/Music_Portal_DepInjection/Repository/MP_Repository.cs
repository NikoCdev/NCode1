using MusicPortal.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicPortal.Repository
{
    public class MP_Repository : IRepository
    {
        private readonly UserContext _context;

        public MP_Repository(UserContext context)
        {
            _context = context;
        }

        // Методы для работы с песнями (Songs)
        public async Task<List<Song>> GetAllSongsAsync()
        {
            return await _context.Songs.Include(s => s.Genre).ToListAsync();
        }

        public async Task<Song> GetSongByIdAsync(int id)
        {
            return await _context.Songs.FindAsync(id);
        }

        public async Task AddSongAsync(Song song)
        {
            await _context.Songs.AddAsync(song);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateSongAsync(Song song)
        {
            _context.Entry(song).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteSongAsync(Song song)
        {
            _context.Songs.Remove(song);
            await _context.SaveChangesAsync();
        }

        // Методы для работы с пользователями (Users)
        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _context.UsersE.ToListAsync();
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _context.UsersE.FindAsync(id);
        }

        public async Task<User> GetUserByLoginAsync(string login)
        {
            return await _context.UsersE.FirstOrDefaultAsync(u => u.Login == login);
        }

        public async Task AddUserAsync(User user)
        {
            await _context.UsersE.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(User user)
        {
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(User user)
        {
            _context.UsersE.Remove(user);
            await _context.SaveChangesAsync();
        }

        // Методы для работы с жанрами (Genres)
        public async Task<List<Genre>> GetAllGenresAsync()
        {
            return await _context.Genres.ToListAsync();
        }

        public async Task<Genre> GetGenreByIdAsync(int id)
        {
            return await _context.Genres.FindAsync(id);
        }

        public async Task AddGenreAsync(Genre genre)
        {
            await _context.Genres.AddAsync(genre);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateGenreAsync(Genre genre)
        {
            _context.Entry(genre).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteGenreAsync(Genre genre)
        {
            _context.Genres.Remove(genre);
            await _context.SaveChangesAsync();
        }
    }
}
