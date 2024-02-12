using System.ComponentModel.DataAnnotations;

namespace BloggWebView.Models
{
    public class UserView
    {
        [Key]
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Name { get; set; }
        public long Phone { get; set; }
        public DateOnly DOB { get; set; }
    }
}
