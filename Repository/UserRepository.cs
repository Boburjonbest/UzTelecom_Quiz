using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using UzTelecom_Quiz.Data;
using UzTelecom_Quiz.Interface;
using UzTelecom_Quiz.Models;


namespace UzTelecom_Quiz.Repository
{
    public class UserRepository : IUser
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        }


        public async Task<User> GetUserByUsernameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }


        public async Task<bool> CreateUserAsync(User user)
        {
            _context.Users.Add(user);

            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async  Task<bool> UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
                return false;

            _context.Users.Remove(user);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> CheckUserCredentialAsync(string username, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
                if(user == null)
                return false;

            return BCrypt.Net.BCrypt.Verify(password, user.Password);
        }

    }
}
