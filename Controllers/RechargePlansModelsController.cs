using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;
using MobileRecharge.Data;
using MobileRecharge.Models;

namespace MobileRecharge.Controllers
{
    public class RechargePlansModelsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public RechargePlansModelsController(ApplicationDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        // GET: RechargePlansModels
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var userId = userManager.GetUserId(this.User);

            var user = await userManager.FindByIdAsync(userId);

            var roles = await userManager.GetRolesAsync(user);

            Console.WriteLine("roles: " + roles.Count);
            foreach(var role in roles)
            {
                if(role.Equals("Admin"))
                {
                    ViewData["Role"] = role;
                }
            }
              return _context.RechargePlans != null ? 
                          View(await _context.RechargePlans.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.RechargePlans'  is null.");
        }

        //filter 
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Index(string provider)
        {
            Console.WriteLine($"Provider : {provider}");
            var userId = userManager.GetUserId(this.User);

            var user = await userManager.FindByIdAsync(userId);

            var roles = await userManager.GetRolesAsync(user);

            Console.WriteLine("roles: " + roles.Count);
            foreach (var role in roles)
            {
                if (role.Equals("Admin"))
                {
                    ViewData["Role"] = role;
                }
            }
            if(provider == "All")
            {
                return _context.RechargePlans != null ?
                        View(await _context.RechargePlans.ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.RechargePlans'  is null.");
            }
            return _context.RechargePlans != null ?
                        View(await _context.RechargePlans.Where(r => r.ProviderName == provider).ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.RechargePlans'  is null.");
        }

        // GET: RechargePlansModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.RechargePlans == null)
            {
                return NotFound();
            }

            var rechargePlansModel = await _context.RechargePlans
                .FirstOrDefaultAsync(m => m.PlanId == id);
            if (rechargePlansModel == null)
            {
                return NotFound();
            }

            return View(rechargePlansModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Details(int? id,string phonenumber)
        {
            Console.WriteLine(id);
            Console.WriteLine(phonenumber);
            var userId = userManager.GetUserId(this.User);

            var user = await userManager.FindByIdAsync(userId);


            




            var rechargePlansModel = await _context.RechargePlans
                .FirstOrDefaultAsync(m => m.PlanId == id);
            if (rechargePlansModel == null)
            {
                return NotFound();
            }
            int validity = Convert.ToInt32(rechargePlansModel.Validity.Split(" ")[0]);

            Console.WriteLine(DateTime.Now.AddDays(validity));
            DateTime ValidTillDateTime = DateTime.Now.AddDays(validity);



            var rechargeReportList = _context.RechargeReports
                .Where(r => r.PhoneNumber == phonenumber)
                .ToList();
            var latestRecharge = rechargeReportList.OrderByDescending(x => x.ReportId).FirstOrDefault();
            
            if(latestRecharge == null || latestRecharge.ValidTill < DateTime.Now)
            {
                RechargeReportModel rechargeReportModel = new RechargeReportModel();
                rechargeReportModel.Plan = rechargePlansModel;
                rechargeReportModel.User = user;
                rechargeReportModel.PhoneNumber = phonenumber;
                rechargeReportModel.DateOfRecharge = DateTime.Now;
                rechargeReportModel.ValidTill = DateTime.Now.AddDays(validity);

                _context.Add(rechargeReportModel);
                await _context.SaveChangesAsync();
            }
            else
            {
                
                return RedirectToAction("Index","RechargeReportModels",new {message = $"You still have a valid Plan till {latestRecharge.ValidTill},Recharge Failed" } );


            }



            return RedirectToAction("Index","RechargeReportModels");
        }

        [Authorize(Roles ="Admin")]
        
        // GET: RechargePlansModels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: RechargePlansModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("PlanId,ProviderName,Calls,Data,Validity,SMS,Price")] RechargePlansModel rechargePlansModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(rechargePlansModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(rechargePlansModel);
        }

        [Authorize(Roles = "Admin")]
        // GET: RechargePlansModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.RechargePlans == null)
            {
                return NotFound();
            }

            var rechargePlansModel = await _context.RechargePlans.FindAsync(id);
            if (rechargePlansModel == null)
            {
                return NotFound();
            }
            return View(rechargePlansModel);
        }

        // POST: RechargePlansModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("PlanId,ProviderName,Calls,Data,Validity,SMS,Price")] RechargePlansModel rechargePlansModel)
        {
            if (id != rechargePlansModel.PlanId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rechargePlansModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RechargePlansModelExists(rechargePlansModel.PlanId))
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
            return View(rechargePlansModel);
        }

        // GET: RechargePlansModels/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.RechargePlans == null)
            {
                return NotFound();
            }

            var rechargePlansModel = await _context.RechargePlans
                .FirstOrDefaultAsync(m => m.PlanId == id);
            if (rechargePlansModel == null)
            {
                return NotFound();
            }

            return View(rechargePlansModel);
        }

        // POST: RechargePlansModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.RechargePlans == null)
            {
                return Problem("Entity set 'ApplicationDbContext.RechargePlans'  is null.");
            }
            var rechargePlansModel = await _context.RechargePlans.FindAsync(id);
            if (rechargePlansModel != null)
            {
                _context.RechargePlans.Remove(rechargePlansModel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RechargePlansModelExists(int id)
        {
          return (_context.RechargePlans?.Any(e => e.PlanId == id)).GetValueOrDefault();
        }
    }
}
