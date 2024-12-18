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
            var secret = "Boburjonbestverymylocationdubai12345";
            var expirationTime = "60";

            if(string.IsNullOrEmpty(expirationTime))
            {
                throw new ArgumentNullException("expirationTime", "Expiration time null bolish keremas. ");
            }

            var expirationTimeInSeconds = 0.0;
            if(!double.TryParse(expirationTime, out expirationTimeInSeconds))
            {
                throw new ArgumentException("Expiration time valid number bolishga majbur.");
            }

            var token = new JwtSecurityToken(
                issuer: "Issuer",
                audience: "Audience",
                expires: DateTime.UtcNow.AddSeconds(expirationTimeInSeconds),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)), SecurityAlgorithms.HmacSha256));

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
