using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceLink.Data;
using ServiceLink.Models;
using ServiceLink.ViewModels;

namespace ServiceLink.Controllers
{
    [Authorize] // all actions require login by default
    public class BookingsController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<BookingsController> _logger;

        public BookingsController(
            ApplicationDbContext db,
            UserManager<ApplicationUser> userManager,
            ILogger<BookingsController> logger)
        {
            _db = db;
            _userManager = userManager;
            _logger = logger;
        }

        // =========================
        // USER: REQUEST BOOKING
        // =========================

        // GET: /Bookings/Create?serviceId=5
        public async Task<IActionResult> Create(int serviceId)
        {
            var service = await _db.Services.FindAsync(serviceId);
            if (service == null)
                return NotFound();

            var vm = new BookingCreateViewModel
            {
                ServiceId = service.ServiceId,
                ServiceTitle = service.Title,
                RequestedFor = DateTime.Today.AddDays(1)
            };

            return View(vm);
        }

        // POST: /Bookings/Create
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookingCreateViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var service = await _db.Services.FindAsync(vm.ServiceId);
            if (service == null)
                return NotFound();

            if (string.IsNullOrEmpty(service.ProviderId))
                return BadRequest("Service is not linked to a provider.");

            var userId = _userManager.GetUserId(User);
            if (userId == null)
                return Forbid();

            var booking = new Booking
            {
                ServiceId = service.ServiceId,
                CustomerId = userId,
                ProviderId = service.ProviderId,
                RequestedFor = vm.RequestedFor,
                Notes = vm.Notes,
                Status = BookingStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };

            _db.Bookings.Add(booking);
            await _db.SaveChangesAsync();

            TempData["SuccessMessage"] =
                "Booking request submitted successfully. Awaiting provider confirmation.";

            return RedirectToAction(nameof(MyBookings));
        }



        // =========================
        // USER: MY BOOKINGS (PENDING PAGE)
        // =========================

        public async Task<IActionResult> MyBookings()
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
                return Forbid();

            var bookings = await _db.Bookings
                .Where(b => b.CustomerId == userId)
                .Include(b => b.Service)
                .OrderByDescending(b => b.CreatedAt)
                .Select(b => new BookingListItemViewModel
                {
                    BookingId = b.BookingId,
                    ServiceTitle = b.Service.Title,
                    RequestedFor = b.RequestedFor,
                    CreatedAt = b.CreatedAt,
                    Status = b.Status,
                    Notes = b.Notes
                })
                .ToListAsync();

            return View(bookings);
        }

        // =========================
        // PROVIDER: INCOMING BOOKINGS
        // =========================

        [Authorize(Roles = "Provider,Admin,MasterDemo")]
        public async Task<IActionResult> Incoming()
        {
            var providerId = _userManager.GetUserId(User);
            if (providerId == null)
                return Forbid();

            var bookings = await _db.Bookings
                .Where(b => b.ProviderId == providerId)
                .Include(b => b.Service)
                .OrderByDescending(b => b.CreatedAt)
                .Select(b => new BookingListItemViewModel
                {
                    BookingId = b.BookingId,
                    ServiceTitle = b.Service.Title,
                    RequestedFor = b.RequestedFor,
                    CreatedAt = b.CreatedAt,
                    Status = b.Status,
                    Notes = b.Notes
                })
                .ToListAsync();

            return View(bookings);
        }

        // =========================
        // USER: CANCEL BOOKING
        // =========================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int id)
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
                return Forbid();

            var booking = await _db.Bookings.FirstOrDefaultAsync(b => b.BookingId == id);
            if (booking == null)
                return NotFound();

            if (booking.CustomerId != userId)
                return Forbid();

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

        // =========================
        // PROVIDER: ACCEPT / REJECT
        // =========================

        [HttpPost]
        [Authorize(Roles = "Provider,Admin,MasterDemo")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Accept(int id)
        {
            var providerId = _userManager.GetUserId(User);
            if (providerId == null)
                return Forbid();

            var booking = await _db.Bookings.FirstOrDefaultAsync(b => b.BookingId == id);
            if (booking == null)
                return NotFound();

            if (booking.ProviderId != providerId)
                return Forbid();

            if (booking.Status != BookingStatus.Pending)
            {
                TempData["ErrorMessage"] = "Booking status has changed.";
                return RedirectToAction(nameof(Incoming));
            }

            booking.Status = BookingStatus.Accepted;
            await _db.SaveChangesAsync();

            TempData["SuccessMessage"] = "Booking accepted.";
            return RedirectToAction(nameof(Incoming));
        }

        [HttpPost]
        [Authorize(Roles = "Provider,Admin,MasterDemo")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reject(int id)
        {
            var providerId = _userManager.GetUserId(User);
            if (providerId == null)
                return Forbid();

            var booking = await _db.Bookings.FirstOrDefaultAsync(b => b.BookingId == id);
            if (booking == null)
                return NotFound();

            if (booking.ProviderId != providerId)
                return Forbid();

            if (booking.Status != BookingStatus.Pending)
            {
                TempData["ErrorMessage"] = "Booking status has changed.";
                return RedirectToAction(nameof(Incoming));
            }

            booking.Status = BookingStatus.Rejected;
            await _db.SaveChangesAsync();

            TempData["SuccessMessage"] = "Booking rejected.";
            return RedirectToAction(nameof(Incoming));
        }
    }
}
