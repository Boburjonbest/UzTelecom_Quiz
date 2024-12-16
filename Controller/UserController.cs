using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UzTelecom_Quiz.Data;
using UzTelecom_Quiz.Interface;
using UzTelecom_Quiz.Models;
using UzTelecom_Quiz.Repository;

namespace UzTelecom_Quiz.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
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

       

       
    }
}
