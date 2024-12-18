using System.Text.Json.Serialization;

namespace UzTelecom_Quiz.Models
{
    public class Login
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

    }
}
