using System.ComponentModel.DataAnnotations;

namespace UzTelecom_Quiz.Models
{
    public class Group
    {
        [Key]
        public string Name { get; set; }
        public int Id { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
