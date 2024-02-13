using System.ComponentModel.DataAnnotations;

namespace RoleBasedLibraryManagement.Models
{
    public class UserDTO
    {
        [Key]
        public required string Username { get; set; }
        public required  string Password { get; set; }
    }
}
