namespace MobileRecharge.Models
{
    public class ContactModel
    {
        [Required]
        [Key]
        public int ContactId { get; set; }

        [Required]

        public string Message { get; set; }

        []
    }
}
