using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using UzTelecom_Quiz.Interface;

namespace UzTelecom_Quiz.Models
{
    public class User 
    {
        public int id { get; set; }
        public string username { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string role { get; set; }
        public int phonenumber { get; set; }
        public string jobtitle { get; set; }
        [JsonIgnore]
        public bool IsPhoneNumberConfirmed { get; set; }
        
        
    }
}
