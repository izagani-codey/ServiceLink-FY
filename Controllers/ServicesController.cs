using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServiceLink.Data;
using ServiceLink.Models;
using ServiceLink.ViewModels;

namespace ServiceLink.Controllers
{
    public class ServicesController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<ServicesController> _logger;

        public ServicesController(ApplicationDbContext db, ILogger<ServicesController> logger)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // GET: /Services
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var list = await _db.Services
                .Where(s => s.IsActive)
                .OrderByDescending(s => s.CreatedAt)
                .AsNoTracking()
                .ToListAsync();

            // ensure non-null
            if (list == null) list = new System.Collections.Generic.List<Service>();

            return View(list);
        }

        // GET: /Services/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var service = await _db.Services
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.ServiceId == id);

            if (service == null) return NotFound();

            return View(service);
        }

        // GET: /Services/Create
        [Authorize(Roles = "Provider,Admin,MasterDemo")]
        public IActionResult Create()
        {
            return View(new ServiceCreateViewModel());
        }

        // POST: /Services/Create
        [HttpPost]
        [Authorize(Roles = "Provider,Admin,MasterDemo")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ServiceCreateViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            try
            {
                var providerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (providerId == null)
                {
                    ModelState.AddModelError("", "Unable to determine current user.");
                    return View(vm);
                }

                var service = new Service
                {
                    ProviderId = providerId,
                    Title = vm.Title,
                    Description = vm.Description,
                    Category = vm.Category,
                    Price = vm.Price,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                _db.Services.Add(service);
                await _db.SaveChangesAsync();

                TempData["SuccessMessage"] = "Service created successfully.";
                return RedirectToAction(nameof(MyServices));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating service");
                ModelState.AddModelError("", "An error occurred while saving. Try again.");
                return View(vm);
            }
        }

        // GET: /Services/MyServices
        [Authorize(Roles = "Provider,Admin,MasterDemo")]
        public async Task<IActionResult> MyServices()
        {
            var providerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (providerId == null) return Forbid();

            var services = await _db.Services
                .Where(s => s.ProviderId == providerId)
                .OrderByDescending(s => s.CreatedAt)
                .AsNoTracking()
                .ToListAsync();

            return View(services);
        }

        // GET: /Services/Edit/5
        [Authorize(Roles = "Provider,Admin,MasterDemo")]
        public async Task<IActionResult> Edit(int id)
        {
            var service = await _db.Services.FirstOrDefaultAsync(s => s.ServiceId == id);
            if (service == null) return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Forbid();
            if (service.ProviderId != userId && !User.IsInRole("Admin") && !User.IsInRole("MasterDemo"))
                return Forbid();

            var vm = new ServiceCreateViewModel
            {
                Title = service.Title,
                Description = service.Description,
                Category = service.Category,
                Price = service.Price
            };

            ViewData["ServiceId"] = service.ServiceId;
            return View(vm);
        }

        // POST: /Services/Edit/5
        [HttpPost]
        [Authorize(Roles = "Provider,Admin,MasterDemo")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ServiceCreateViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                ViewData["ServiceId"] = id;
                return View(vm);
            }

            var service = await _db.Services.FirstOrDefaultAsync(s => s.ServiceId == id);
            if (service == null) return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Forbid();
            if (service.ProviderId != userId && !User.IsInRole("Admin") && !User.IsInRole("MasterDemo"))
                return Forbid();

            service.Title = vm.Title;
            service.Description = vm.Description;
            service.Category = vm.Category;
            service.Price = vm.Price;

            try
            {
                await _db.SaveChangesAsync();
                TempData["SuccessMessage"] = "Service updated successfully.";
                return RedirectToAction(nameof(MyServices));
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Concurrency error editing service {Id}", id);
                ModelState.AddModelError("", "This record was modified by someone else. Please reload and try again.");
                ViewData["ServiceId"] = id;
                return View(vm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving service edit {Id}", id);
                ModelState.AddModelError("", "An error occurred while saving. Try again.");
                ViewData["ServiceId"] = id;
                return View(vm);
            }
        }

        // GET: /Services/Delete/5
        [Authorize(Roles = "Provider,Admin,MasterDemo")]
        public async Task<IActionResult> Delete(int id)
        {
            var service = await _db.Services.AsNoTracking().FirstOrDefaultAsync(s => s.ServiceId == id);
            if (service == null) return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Forbid();
            if (service.ProviderId != userId && !User.IsInRole("Admin") && !User.IsInRole("MasterDemo"))
                return Forbid();

            return View(service);
        }

        // POST: /Services/DeleteConfirmed/5
        [HttpPost, ActionName("DeleteConfirmed")]
        [Authorize(Roles = "Provider,Admin,MasterDemo")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var service = await _db.Services.FirstOrDefaultAsync(s => s.ServiceId == id);
            if (service == null) return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Forbid();
            if (service.ProviderId != userId && !User.IsInRole("Admin") && !User.IsInRole("MasterDemo"))
                return Forbid();

            try
            {
                _db.Services.Remove(service);
                await _db.SaveChangesAsync();
                TempData["SuccessMessage"] = "Service deleted.";
                return RedirectToAction(nameof(MyServices));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting service {Id}", id);
                TempData["ErrorMessage"] = "Could not delete service. Try again later.";
                return RedirectToAction(nameof(MyServices));
            }
        }
    }
}
