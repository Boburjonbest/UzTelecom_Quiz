using UzTelecom_Quiz.Models;

namespace UzTelecom_Quiz.Interface
{
    public interface IUser
    {
        Task<User> GetUserByIdAsync(int id);
        Task<User> GetUserByUsernameAsync(string username);
        Task<bool> CreateUserAsync(User user);
        Task<bool> UpdateUserAsync(User user);
        Task<bool> DeleteUserAsync(int id);
        Task<bool> CheckUserCredentialAsync(string username, string password);
    }
}
