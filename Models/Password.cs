namespace UzTelecom_Quiz.Models
{
    public class Password
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string PasswordHash { get; set; }
        public  DateTime CreatedAt { get; set; }
    }
}
