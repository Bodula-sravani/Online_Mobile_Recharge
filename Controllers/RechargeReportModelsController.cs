using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MobileRecharge.Data;
using MobileRecharge.Models;

namespace MobileRecharge.Controllers
{

    public class RechargeReportViewModel
    {
        public int ReportId { get; set; }

        public string Phonenumber { get; set; }
        public DateTime DateOfRecharge { get; set; }

        public int PlanId { get; set; }

        public string UserId { get; set; }

        public string UserName { get; set; }
        public DateTime ValidTill { get; set; }
    }
    public class RechargeReportModelsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public RechargeReportModelsController(ApplicationDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        // GET: RechargeReportModels
        public async Task<IActionResult> Index(string message)
        {
            Console.WriteLine("message " + message);
            ViewData["failed"] = message;
            var userId = userManager.GetUserId(this.User);

            var user = await userManager.FindByIdAsync(userId);

            var roles = await userManager.GetRolesAsync(user);

            Console.WriteLine("roles: " + roles.Count);
            foreach (var role in roles)
            {
                if (role.Equals("Admin"))
                {
                    ViewData["Role"] = role;
                    var rechargeReportListAdmin = _context.RechargeReports.Include(r => r.Plan).Include(r => r.User).Select(r => new RechargeReportViewModel
                    {
                        ReportId = r.ReportId,
                        Phonenumber = r.PhoneNumber,
                        DateOfRecharge = r.DateOfRecharge,
                        UserId = r.User.Id,
                        PlanId = r.Plan.PlanId,
                        UserName = r.User.UserName,

                         ValidTill = r.ValidTill

                    }).ToList();

                    return View(rechargeReportListAdmin);

                }
            }

           //var rechargeReports = await _context.RechargeReports.ToListAsync();
           //if(rechargeReports.Count == 0)
           // {
           //     return Problem("Entity set 'ApplicationDbContext.RechargeReports'  is null.");
           // }
            var rechargeReportListUser = _context.RechargeReports.Include(r => r.Plan).Include(r => r.User).Select(r => new RechargeReportViewModel
            {
                ReportId = r.ReportId,
                Phonenumber = r.PhoneNumber,
                DateOfRecharge = r.DateOfRecharge,
                UserId = r.User.Id,
                PlanId = r.Plan.PlanId,
                UserName = r.User.UserName,
                ValidTill = r.ValidTill



            }).Where(r => r.UserId == userId).ToList();









            return View(rechargeReportListUser);
        }

        // GET: RechargeReportModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.RechargeReports == null)
            {
                return NotFound();
            }

            var rechargeReportModel = await _context.RechargeReports
                .FirstOrDefaultAsync(m => m.ReportId == id);
            if (rechargeReportModel == null)
            {
                return NotFound();
            }

            return View(rechargeReportModel);
        }

        // GET: RechargeReportModels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: RechargeReportModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ReportId,PhoneNumber,DateOfRecharge")] RechargeReportModel rechargeReportModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(rechargeReportModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(rechargeReportModel);
        }

        // GET: RechargeReportModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.RechargeReports == null)
            {
                return NotFound();
            }

            var rechargeReportModel = await _context.RechargeReports.FindAsync(id);
            if (rechargeReportModel == null)
            {
                return NotFound();
            }
            return View(rechargeReportModel);
        }

        // POST: RechargeReportModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ReportId,PhoneNumber,DateOfRecharge")] RechargeReportModel rechargeReportModel)
        {
            if (id != rechargeReportModel.ReportId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rechargeReportModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RechargeReportModelExists(rechargeReportModel.ReportId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(rechargeReportModel);
        }

        // GET: RechargeReportModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.RechargeReports == null)
            {
                return NotFound();
            }

            var rechargeReportModel = await _context.RechargeReports
                .FirstOrDefaultAsync(m => m.ReportId == id);
            if (rechargeReportModel == null)
            {
                return NotFound();
            }

            return View(rechargeReportModel);
        }

        // POST: RechargeReportModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.RechargeReports == null)
            {
                return Problem("Entity set 'ApplicationDbContext.RechargeReports'  is null.");
            }
            var rechargeReportModel = await _context.RechargeReports.FindAsync(id);
            if (rechargeReportModel != null)
            {
                _context.RechargeReports.Remove(rechargeReportModel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RechargeReportModelExists(int id)
        {
          return (_context.RechargeReports?.Any(e => e.ReportId == id)).GetValueOrDefault();
        }
    }
}
