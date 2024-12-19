using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace UzTelecom_Quiz.Service
{
   public class TokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(string username, string role)
        {
            var claims = new List<Claim>()
           {
               new Claim(ClaimTypes.Name, username),
               new Claim(ClaimTypes.Role, role)
           };

           var token = new JwtSecurityToken(
               issuer: "Issuer",
               audience: "Audience",
               claims: claims,
               expires: DateTime.UtcNow.AddDays(7),
               signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Boburjonbestverymylocationdubai12345")), SecurityAlgorithms.HmacSha256));

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public bool ValidateToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _configuration["Jwt:Issuer"],
                    ValidAudience = _configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };

                tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
                return validatedToken != null;

            }
            catch
            {
                return false;
            }
        }
    }




}
