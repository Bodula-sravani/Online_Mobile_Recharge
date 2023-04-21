using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MobileRecharge.Models;

namespace MobileRecharge.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<ContactModel> ContactDetails { get; set; }
        public DbSet<RechargePlansModel>? RechargePlans { get; set; }
        public DbSet<RechargeReportModel>? RechargeReports { get; set; }
    }
}