using MusicPortal.Models;
namespace MusicPortal.Repository
{

    public interface IRepository
    {
        Task<List<Song>> GetAllSongsAsync();
        Task<Song> GetSongByIdAsync(int id);
        Task AddSongAsync(Song message);
        Task UpdateSongAsync(Song message);
        Task DeleteSongAsync(Song message);

        Task<List<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(int id);
        Task<User> GetUserByLoginAsync(string login);
        Task AddUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(User user);
        
        Task<List<Genre>> GetAllGenresAsync();
        Task<Genre> GetGenreByIdAsync(int id);       
        Task AddGenreAsync(Genre user);
        Task UpdateGenreAsync(Genre user);
        Task DeleteGenreAsync(Genre user);
    }

}