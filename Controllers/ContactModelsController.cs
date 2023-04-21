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
    public class ContactModelsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public ContactModelsController(ApplicationDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        // GET: ContactModels
        public async Task<IActionResult> Index()
        {
            var userId = userManager.GetUserId(this.User);
            Console.WriteLine("userid: " + userId);
            var user = await userManager.FindByIdAsync(userId);
            return _context.ContactDetails != null ? 
                          View(await _context.ContactDetails.Where(c => c.User.Equals(user)).ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.ContactDetails'  is null.");
        }

        // GET: ContactModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ContactDetails == null)
            {
                return NotFound();
            }

            var contactModel = await _context.ContactDetails
                .FirstOrDefaultAsync(m => m.ContactId == id);
            if (contactModel == null)
            {
                return NotFound();
            }

            return View(contactModel);
        }

        // GET: ContactModels/Create
        public IActionResult Create()
        {
            //var userId = userManager.GetUserId(this.User);

            //var user = await userManager.FindByIdAsync(userId);

            //var roles = await userManager.GetRolesAsync(user);

            //foreach (var role in roles)
            //{
            //    if (role.Equals("Admin"))
            //    {
            //        ViewData["Role"] = role;
            //    }
            //}

            return View();
        }

        // POST: ContactModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ContactId,Message,DateOfMessage,Reply")] ContactModel contactModel)
        {
            var userId = userManager.GetUserId(this.User);

            var user = await userManager.FindByIdAsync(userId);

            contactModel.User=user;
            contactModel.DateOfMessage = DateTime.Now;
            contactModel.Reply = "";
            Console.WriteLine("id: " + contactModel.ContactId);
            Console.WriteLine("message: " + contactModel.Message);
            Console.WriteLine("date: " + contactModel.DateOfMessage);
            Console.WriteLine("Reply: " + contactModel.Reply);
            Console.WriteLine("user: " + contactModel.User);
            //var roles = await userManager.GetRolesAsync(user);
            Console.WriteLine("IN create post");
            Console.WriteLine("is valid: "+ModelState.IsValid);
            Console.WriteLine("is valid");
            _context.Add(contactModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: ContactModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ContactDetails == null)
            {
                return NotFound();
            }

            var contactModel = await _context.ContactDetails.FindAsync(id);
            if (contactModel == null)
            {
                return NotFound();
            }
            return View(contactModel);
        }

        // POST: ContactModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ContactId,Message,DateOfMessage,Reply")] ContactModel contactModel)
        {
            if (id != contactModel.ContactId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contactModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContactModelExists(contactModel.ContactId))
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
            return View(contactModel);
        }

        // GET: ContactModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ContactDetails == null)
            {
                return NotFound();
            }

            var contactModel = await _context.ContactDetails
                .FirstOrDefaultAsync(m => m.ContactId == id);
            if (contactModel == null)
            {
                return NotFound();
            }

            return View(contactModel);
        }

        // POST: ContactModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ContactDetails == null)
            {
                return Problem("Entity set 'ApplicationDbContext.ContactDetails'  is null.");
            }
            var contactModel = await _context.ContactDetails.FindAsync(id);
            if (contactModel != null)
            {
                _context.ContactDetails.Remove(contactModel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContactModelExists(int id)
        {
          return (_context.ContactDetails?.Any(e => e.ContactId == id)).GetValueOrDefault();
        }
    }
}
