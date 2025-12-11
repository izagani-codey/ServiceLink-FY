using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceLink.Data;
using ServiceLink.Models;
using ServiceLink.ViewModels;

namespace ServiceLink.Controllers
{
    public class BookingsController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<BookingsController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        public BookingsController(ApplicationDbContext db, ILogger<BookingsController> logger, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _logger = logger;
            _userManager = userManager;
        }

        // GET: /Bookings/Create?serviceId=5
        [Authorize]
        public async Task<IActionResult> Create(int serviceId)
        {
            var service = await _db.Services.AsNoTracking().FirstOrDefaultAsync(s => s.ServiceId == serviceId);
            if (service == null) return NotFound();

            var vm = new BookingCreateViewModel
            {
                ServiceId = serviceId,
                ServiceTitle = service.Title,
                RequestedFor = DateTime.UtcNow.Date.AddDays(1) // default tomorrow
            };

            return View(vm);
        }

        // POST: /Bookings/Create
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookingCreateViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            if (vm.RequestedFor.Date < DateTime.UtcNow.Date)
            {
                ModelState.AddModelError(nameof(vm.RequestedFor), "Requested date cannot be in the past.");
                return View(vm);
            }

            var service = await _db.Services.FirstOrDefaultAsync(s => s.ServiceId == vm.ServiceId);
            if (service == null) return NotFound();

            var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (customerId == null) return Forbid();

            var booking = new Booking
            {
                ServiceId = vm.ServiceId,
                CustomerId = customerId,
                ProviderId = service.ProviderId ?? throw new InvalidOperationException("Service has no provider."),
                RequestedFor = vm.RequestedFor,
                Notes = vm.Notes,
                Status = BookingStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };

            _db.Bookings.Add(booking);
            await _db.SaveChangesAsync();

            TempData["SuccessMessage"] = "Booking requested. Provider will review your request.";
            return RedirectToAction(nameof(MyBookings));
        }

        // GET: /Bookings/MyBookings
        [Authorize]
        public async Task<IActionResult> MyBookings()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Forbid();

            var bookings = await _db.Bookings
                .AsNoTracking()
                .Where(b => b.CustomerId == userId)
                .Include(b => b.Service)
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();

            return View(bookings);
        }

        // POST: /Bookings/Cancel/5
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Forbid();

            var booking = await _db.Bookings.FirstOrDefaultAsync(b => b.BookingId == id);
            if (booking == null) return NotFound();

            if (booking.CustomerId != userId) return Forbid();

            if (booking.Status != BookingStatus.Pending)
            {
                TempData["ErrorMessage"] = "Only pending bookings can be cancelled.";
                return RedirectToAction(nameof(MyBookings));
            }

            booking.Status = BookingStatus.Cancelled;
            await _db.SaveChangesAsync();

            TempData["SuccessMessage"] = "Booking cancelled.";
            return RedirectToAction(nameof(MyBookings));
        }

        // GET: /Bookings/Incoming
        [Authorize(Roles = "Provider,Admin,MasterDemo")]
        public async Task<IActionResult> Incoming()
        {
            var providerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (providerId == null) return Forbid();

            var bookings = await _db.Bookings
                .AsNoTracking()
                .Where(b => b.ProviderId == providerId && b.Status == BookingStatus.Pending)
                .Include(b => b.Service)
                .OrderBy(b => b.RequestedFor)
                .ToListAsync();

            // build viewmodels including customer info
            var vmList = new List<BookingListItemViewModel>();
            foreach (var b in bookings)
            {
                var user = await _userManager.FindByIdAsync(b.CustomerId);
                vmList.Add(new BookingListItemViewModel
                {
                    BookingId = b.BookingId,
                    ServiceId = b.ServiceId,
                    ServiceTitle = b.Service?.Title ?? string.Empty,
                    RequestedFor = b.RequestedFor,
                    Status = b.Status,
                    CustomerId = b.CustomerId,
                    CustomerEmail = user?.Email ?? b.CustomerId,
                    CreatedAt = b.CreatedAt,
                    Notes = b.Notes
                });
            }

            return View(vmList);
        }

        // POST: /Bookings/Accept/5
        [HttpPost]
        [Authorize(Roles = "Provider,Admin,MasterDemo")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Accept(int id)
        {
            var providerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (providerId == null) return Forbid();

            var booking = await _db.Bookings.FirstOrDefaultAsync(b => b.BookingId == id);
            if (booking == null) return NotFound();

            if (booking.ProviderId != providerId) return Forbid();
            if (booking.Status != BookingStatus.Pending)
            {
                TempData["ErrorMessage"] = "Booking status has changed. Action not allowed.";
                return RedirectToAction(nameof(Incoming));
            }

            booking.Status = BookingStatus.Accepted;
            await _db.SaveChangesAsync();

            TempData["SuccessMessage"] = "Booking accepted.";
            return RedirectToAction(nameof(Incoming));
        }

        // POST: /Bookings/Reject/5
        [HttpPost]
        [Authorize(Roles = "Provider,Admin,MasterDemo")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reject(int id)
        {
            var providerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (providerId == null) return Forbid();

            var booking = await _db.Bookings.FirstOrDefaultAsync(b => b.BookingId == id);
            if (booking == null) return NotFound();

            if (booking.ProviderId != providerId) return Forbid();
            if (booking.Status != BookingStatus.Pending)
            {
                TempData["ErrorMessage"] = "Booking status has changed. Action not allowed.";
                return RedirectToAction(nameof(Incoming));
            }

            booking.Status = BookingStatus.Rejected;
            await _db.SaveChangesAsync();

            TempData["SuccessMessage"] = "Booking rejected.";
            return RedirectToAction(nameof(Incoming));
        }
    }
}
