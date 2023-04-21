using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace MobileRecharge.Models
{
    public class RechargeReportModel
    {
        [Required]
        [Key]
        public int ReportId { get; set; }

        [ForeignKey("PlanId")]
        public RechargePlansModel Plan { get; set; }

        public IdentityUser User { get; set; }

        [Required]
        public DateTime DateOfRecharge { get; set; }




    }
}
