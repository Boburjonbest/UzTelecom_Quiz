using BCrypt.Net;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UzTelecom_Quiz.Data;
using UzTelecom_Quiz.Interface;
using UzTelecom_Quiz.Models;


namespace UzTelecom_Quiz.Repository
{
    public class UserRepository : IUserService
    {
        private readonly ApplicationDbContext _context;
        private static readonly Dictionary<string, string> _verificationCodes = new(); // Подтвердить код 

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User> CreateUserAsync(User user)
        {
            try
            {
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
                return user;
            }
            catch (Exception ex) 
            {
                throw new Exception("Foydalanuvchi yaratishda xato yuz berdi", ex);
            }
        }


        public async Task<bool> SendPhoneNumberVerificationCodeAsync(int phonenumber)
        {
           var phoneNumberString = phonenumber.ToString();
           var random = new Random();
           var code = random.Next(100000, 999999).ToString();
            if (_verificationCodes.ContainsKey(phoneNumberString))
                _verificationCodes[phoneNumberString] = code;
            else 
                _verificationCodes.Add(phoneNumberString, code);

            Console.WriteLine($"Kod {code} telefon raqamga {phoneNumberString} yuborildi. ");
            return true;

        }

        public async Task<bool> ConfirmPhoneNumberAsync(int phonenumber, string code)
        {
            var phoneNumberString = phonenumber.ToString();

            if (_verificationCodes.ContainsKey(phoneNumberString) && _verificationCodes[phoneNumberString] == code)
            {
                _verificationCodes.Remove(phoneNumberString);
                return true;
            }

            return false;
        }

        public async Task<bool> CreatePasswordAsync(string username, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.username == username);
            if(user == null || !user.IsPhoneNumberConfirmed)
                return false;

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

            var passwordEntry = new Password
            {
                UserId = user.id,
                PasswordHash = passwordHash,
                CreatedAt = DateTime.UtcNow
            };

            _context.Passwords.Add(passwordEntry);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CheckPasswordAsync(string username, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.username == username);
            if(user == null)
                return false;

            var passwordEntry = await _context.Passwords
                .FirstOrDefaultAsync(p => p.UserId == user.id);
            if (passwordEntry == null)
                return false;

            return BCrypt.Net.BCrypt.Verify(password, passwordEntry.PasswordHash);
        }

        public async Task<bool> CheckRoleAsync(User user, string role)
        {
            return user.role.Equals(role, StringComparison.OrdinalIgnoreCase);
        }

       
        public string GenerateToken(User user)
        {
           var tokenHandler = new JwtSecurityTokenHandler();
           var key = Encoding.ASCII.GetBytes("Boburjonbestverymylocationdubai12345");

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.username),
                    new Claim(ClaimTypes.Role, user.role),
                    new Claim("PhoneNumber", user.phonenumber.ToString()),
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),SecurityAlgorithms.HmacSha256Signature),
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<bool> GetUserByLoginAsync(string username)
        {
            return await _context.Users.AnyAsync(u => u.username == username);
        }

       
    }
}
