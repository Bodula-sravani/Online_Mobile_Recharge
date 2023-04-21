using System.ComponentModel.DataAnnotations;

namespace MobileRecharge.Models
{
    public class RechargePlansModel
    {
        [Required]
        [Key]
        public int PlanId { get; set; }

        [Required]
        public string ProviderName { get; set; }

        [Required]
        public string Calls { get; set; }

        [Required]
        public string Data { get; set; }

        [Required]
        public string Validity { get; set; }

        [Required]
        public string SMS { get; set; }

        [Required]
        public int Price { get; set; }
    }
}
