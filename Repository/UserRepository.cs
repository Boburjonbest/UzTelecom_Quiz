using BCrypt.Net;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
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
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;
        private static readonly Dictionary<string, string> _verificationCodes = new(); // Подтвердить код 

        public UserRepository(IConfiguration configuration,ApplicationDbContext context)
        {
            _configuration = configuration;
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

        public async Task<bool> CheckRoleAsync(User user, string role)
        {
            return user.role.Equals(role, StringComparison.OrdinalIgnoreCase);
        }

        public async Task<bool> GetUserByLoginAsync(string username)
        {
            return await _context.Users.AnyAsync(u => u.username == username);
        }

        public async Task<bool> ValidateTokenAsync(string token)
        {
           try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);

                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _configuration["Jwt:Issuer"],
                    ValidAudience = _configuration["Jwt: Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                }, out SecurityToken validatedToken);
                return validatedToken != null;
                
            }
            catch
            {
                return false;
            }
        }

    }
}
