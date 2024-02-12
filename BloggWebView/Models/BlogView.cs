using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BloggWebView.Models
{
    public class BlogView
    {
        [Key]
        public required String Title { get; set; }
        [ForeignKey(nameof(Username))]
        public required String Username { get; set; }
        public required string Description { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
