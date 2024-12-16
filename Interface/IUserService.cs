using UzTelecom_Quiz.Models;
using UzTelecom_Quiz.Repository;

namespace UzTelecom_Quiz.Interface
{
    public interface IUserService 
    {
        Task<User> CreateUserAsync(User user );
        Task<bool> SendPhoneNumberVerificationCodeAsync(int phonenumber);
        Task<bool> ConfirmPhoneNumberAsync(int phonenumber, string code);
        Task<bool> CreatePasswordAsync(string login, string password);
        Task<bool> CheckPasswordAsync(string login, string password); // Проверка пароля
        Task<bool> GetUserByLoginAsync(string login);
        Task<bool> CheckRoleAsync(User user, string role); // Проверка роль
        string GenerateToken(User user);
    }
}
