using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace MobileRecharge.Models
{
    public class ContactModel
    {
        [Key]
        [Required]
        public int ContactId { get; set; }
        public string Message { get; set; }
        public IdentityUser User { get; set; }

        public DateTime DateOfMessage { get; set; }

        public string Reply { get; set; }
    }
}
