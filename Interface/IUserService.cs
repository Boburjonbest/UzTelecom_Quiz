using UzTelecom_Quiz.Models;
using UzTelecom_Quiz.Repository;

namespace UzTelecom_Quiz.Interface
{
    public interface IUserService 
    {
        Task<User> CreateUserAsync(User user );
        Task<bool> SendPhoneNumberVerificationCodeAsync(int phonenumber);
         // Проверка пароля
        Task<bool> GetUserByLoginAsync(string username);
        Task<bool> CheckRoleAsync(User user, string role); // Проверка роль
        Task<bool> ValidateTokenAsync(string token); // Проверка Токена для Паролья
    }
}
