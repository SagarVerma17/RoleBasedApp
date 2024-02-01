using System.ComponentModel.DataAnnotations;

namespace RoleBasedLibraryManagement.Models
{
    public class User
    {
        [Key]
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateOnly DOB { get; set; }
    }
}
