using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoleBasedApp.Models
{
    public class Blog
    {
        [Key]
        public required String Title { get; set; }
        [ForeignKey(nameof(Username))]
        public required String Username { get; set; }
        public required string Description { get; set; }
        public  DateTime TimeStamp { get; set; }
    }
}
