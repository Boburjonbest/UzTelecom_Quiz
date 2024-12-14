using System.Text.RegularExpressions;
using UzTelecom_Quiz.Interface;

namespace UzTelecom_Quiz.Models
{
    public class User 
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public string? Phone_Number { get; set; }
        public virtual Group Group { get; set; }
    }
}
