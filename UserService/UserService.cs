using UzTelecom_Quiz.Interface;

namespace UzTelecom_Quiz.UserService
{
    public class UserService
    {
        private readonly IUser user;

        public UserService(IUser user)
        { 
            this.user = user; 
        }

        public async Task<bool> LoginAsync(string username, string password)
        {
            return await user.CheckUserCredentialAsync(username, password);
        }
    }
}
