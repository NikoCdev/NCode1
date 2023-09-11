using Gbook.Models;
namespace Gbook.Repository
{
   
        public interface IRepository
        {
            Task<List<Message>> GetAllMessagesAsync();
            Task<Message> GetMessageByIdAsync(int id);
            Task AddMessageAsync(Message message);
            Task UpdateMessageAsync(Message message);
            Task DeleteMessageAsync(Message message);

            Task<List<User>> GetAllUsersAsync();
            Task<User> GetUserByIdAsync(int id);
            Task<User> GetUserByLoginAsync(string login);
            Task AddUserAsync(User user);
            Task UpdateUserAsync(User user);
            Task DeleteUserAsync(User user);
        }
    
}
