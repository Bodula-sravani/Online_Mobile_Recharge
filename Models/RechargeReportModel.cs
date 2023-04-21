using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace MobileRecharge.Models
{
    public class RechargeReportModel
    {
        [Required]
        [Key]
        public int RechargeReportId { get; set; }

        [Required]
        public int ReportPlanId { get; set; }
        public RechargePlansModel Plan { get; set; }

        [Required]
        public int ReportUserId { get; set; }

        [ForeignKey("ReportUserId")]
        public virtual AspNetUsers user { get; set; }


        [Required]
        public string PhoneNumber { get; set; }




    }
}
