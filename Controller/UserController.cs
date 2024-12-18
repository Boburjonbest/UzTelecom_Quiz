using BCrypt.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UzTelecom_Quiz.Data;
using UzTelecom_Quiz.Interface;
using UzTelecom_Quiz.Models;
using UzTelecom_Quiz.Repository;
using UzTelecom_Quiz.Service;

namespace UzTelecom_Quiz.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly TokenService _tokenService;
        private readonly ApplicationDbContext _context;

        public UserController(IUserService userService, TokenService tokenService, ApplicationDbContext context)
        {
            _userService = userService;
            _tokenService = tokenService;
            _context = context;
        }
        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser([FromBody] User user)
        {
            if (!ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }
            try
            {
                var createdUser = await _userService.CreateUserAsync(user);
                return Ok(createdUser);
            }
            catch (Exception ex) 
            {
                return BadRequest(new {message = ex.Message});
            }
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] Register register)
        {
           if(register == null || string.IsNullOrEmpty(register.Username) || string.IsNullOrEmpty(register.Password))
            {
                return BadRequest("Foydalanuvchi va Parol talab qilinadi");
            }

            var existingUser = await _context.Logins
                 .FirstOrDefaultAsync(u => u.Username == register.Username);

            if(existingUser != null)
            {
                return Conflict(" Bunaqa Foydalanuvchi  mavjud. ");
            }

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(register.Password);

            var newUser = new Register
            {
                Username = register.Username,
                Password = hashedPassword,
                Role = register.Role,

            };

            _context.Registers.Add(newUser);
            await _context.SaveChangesAsync();

            return Ok("Foydalanuvchi muvaffaqiyatli ro`yhatdan o`tdi");  
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] Login login)
        {
            if(login == null || string.IsNullOrEmpty(login.Username) || string.IsNullOrEmpty(login.Password))
            {
                return BadRequest("Username va Login kiritsh kerak");
            }

            var user = await _context.Registers
                .FirstOrDefaultAsync(u => u.Username == login.Username);

            if(user == null)
            {
                return Unauthorized("Xato Username yoki Password");
            }
            
            if(!BCrypt.Net.BCrypt.Verify(login.Password, user.Password))
            {
                return Unauthorized("Xato Username yoki Password");
            }

            var token = _tokenService.GenerateToken(login.Username, user.Role);

            _context.Logins.Add(login);
            await _context.SaveChangesAsync();

            return Ok(new { 
                Token = token,
                Role = user.Role
                
            });
        }
        

            

        

        




    }
}
