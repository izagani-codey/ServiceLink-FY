using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            _db = db;
            _logger = logger;
        }

        // GET: /Services
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var list = await _db.Services
                .Where(s => s.IsActive)
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync();

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
        [Authorize(Roles = "Provider")]
        public IActionResult Create()
        {
            return View(new ServiceCreateViewModel());
        }

        // POST: /Services/Create
        [HttpPost]
        [Authorize(Roles = "Provider")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ServiceCreateViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            try
            {
                // get current user id
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
        [Authorize(Roles = "Provider")]
        public async Task<IActionResult> MyServices()
        {
            var providerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (providerId == null) return Forbid();

            var services = await _db.Services
                .Where(s => s.ProviderId == providerId)
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync();

            return View(services);
        }

        // NOTE: Edit/Delete endpoints will be added later (we'll keep ownership checks then).
    }
}
